using System;
using System.IO;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error
}

public class SandyLogger : IDisposable
{
    private static StreamWriter _sharedLogWriter;
    private static readonly object _lock = new object();
    private static string _logFilePath;
    private static readonly List<SandyLogger> _activeLoggers = new List<SandyLogger>();
    private readonly string _typeName;
    private readonly ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();
    private bool _isWriting = false;
    private bool _isDisposed = false;

    // 로그 파일 관리 설정
    private const int MAX_LOG_FILES = 50;
    private const long MAX_FILE_SIZE = 10 * 1024 * 1024; // 10MB
    private const int MAX_RETENTION_DAYS = 30;

#if ENABLE_LOG
    private LogLevel _minLogLevel = LogLevel.Debug;
#else
#if UNITY_EDITOR
    private LogLevel _minLogLevel = LogLevel.Debug;
#else
    private LogLevel _minLogLevel = LogLevel.Warning;
#endif
#endif
    private bool _enableFileLogging = true;
    private bool _enableConsoleLogging = true;

    private static bool _isInitialized = false;
    private static bool _isClosed = false;

    public SandyLogger(Type type)
    {
        _typeName = type.Name;

        lock (_lock)
        {
            _activeLoggers.Add(this);
        }
    }

    private static void InitializeSharedWriter()
    {
        if (_isInitialized)
            return;

        UnityEngine.Debug.Log("InitializeSharedWriter");

        string logDirectory = Path.Combine(Application.persistentDataPath, "Logs");
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        // 오래된 로그 파일 정리
        CleanupOldLogs(logDirectory);

        string dateTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        _logFilePath = Path.Combine(logDirectory, $"game_log_{dateTime}.txt");

        try
        {
            _sharedLogWriter = new StreamWriter(_logFilePath, true);
            _sharedLogWriter.AutoFlush = true;
            _sharedLogWriter.WriteLine("Log File Initialized");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Failed to initialize log file: {e.Message}");
        }

        _isInitialized = true;
    }

    private static void CleanupOldLogs(string logDirectory)
    {
        try
        {
            var logFiles = Directory.GetFiles(logDirectory, "game_log_*.txt")
                                  .Select(f => new FileInfo(f))
                                  .OrderByDescending(f => f.CreationTime)
                                  .ToList();

            // 파일 수 제한
            if (logFiles.Count > MAX_LOG_FILES)
            {
                foreach (var file in logFiles.Skip(MAX_LOG_FILES))
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError($"Failed to delete old log file {file.Name}: {e.Message}");
                    }
                }
            }

            // 파일 크기 제한
            foreach (var file in logFiles.Take(MAX_LOG_FILES))
            {
                if (file.Length > MAX_FILE_SIZE)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError($"Failed to delete oversized log file {file.Name}: {e.Message}");
                    }
                }
            }

            // 보관 기간 제한
            var cutoffDate = DateTime.Now.AddDays(-MAX_RETENTION_DAYS);
            foreach (var file in logFiles)
            {
                if (file.CreationTime < cutoffDate)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError($"Failed to delete expired log file {file.Name}: {e.Message}");
                    }
                }
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Failed to cleanup old logs: {e.Message}");
        }
    }

    public void SetMinLogLevel(LogLevel level)
    {
#if !ENABLE_LOG
        if (level < LogLevel.Warning)
        {
            UnityEngine.Debug.LogWarning("릴리즈 빌드에서는 Warning 레벨 이하로 설정할 수 없습니다.");
            return;
        }
#endif
        _minLogLevel = level;
    }

    public void EnableFileLogging(bool enable)
    {
        _enableFileLogging = enable;
    }

    public void EnableConsoleLogging(bool enable)
    {
        _enableConsoleLogging = enable;
    }

    public void Log(LogLevel level, string message, UnityEngine.Object context = null)
    {
        if (_isClosed)
        {
            UnityEngine.Debug.LogError("Logger is closed");
            return;
        }

        if (level < _minLogLevel)
            return;

        string formattedMessage = FormatLogMessage(level, message);

        if (_enableConsoleLogging)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    UnityEngine.Debug.Log(formattedMessage, context);
                    break;
                case LogLevel.Info:
                    UnityEngine.Debug.Log(formattedMessage, context);
                    break;
                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning(formattedMessage, context);
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogError(formattedMessage, context);
                    break;
            }
        }

        if (_enableFileLogging)
        {
            InitializeSharedWriter();
            WriteToFile(formattedMessage);
        }
    }

    private string FormatLogMessage(LogLevel level, string message)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        return $"[{timestamp}] [{level}] [{_typeName}] {message}";
    }

    private void WriteToFile(string message)
    {
        if (!_enableFileLogging || _sharedLogWriter == null)
            return;

        _logQueue.Enqueue(message);

        if (!_isWriting)
        {
            _isWriting = true;
            ProcessLogQueue().Forget();
        }
    }

    private async UniTaskVoid ProcessLogQueue()
    {
        try
        {
            while (!_logQueue.IsEmpty && !_isDisposed)
            {
                if (_logQueue.TryDequeue(out string message))
                {
                    try
                    {
                        await UniTask.SwitchToThreadPool();
                        _sharedLogWriter.WriteLine(message);
                        await UniTask.SwitchToMainThread();
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError($"Failed to write log to file: {e.Message}");
                        _enableFileLogging = false;
                        break;
                    }
                }
            }
            _isWriting = false;
        }
        catch (Exception e)
        {
            _isWriting = false;
        }
    }

    // 편의 메서드들
    public void Debug(string message, UnityEngine.Object context = null)
    {
        Log(LogLevel.Debug, message, context);
    }

    public void Info(string message, UnityEngine.Object context = null)
    {
        Log(LogLevel.Info, message, context);
    }

    public void Warning(string message, UnityEngine.Object context = null)
    {
        Log(LogLevel.Warning, message, context);
    }

    public void Error(string message, UnityEngine.Object context = null)
    {
        Log(LogLevel.Error, message, context);
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;
        lock (_lock)
        {
            _activeLoggers.Remove(this);
        }

        if (_sharedLogWriter != null)
        {
            try
            {
                ProcessLogQueue().Forget();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Failed to process final log messages: {e.Message}");
            }
        }
    }

    public static async UniTaskVoid CloseSharedWriter()
    {
        if (_isClosed)
            return;

        _isClosed = true;

        if (_sharedLogWriter != null)
        {
            try
            {
                // 모든 SandyLogger 인스턴스의 로그 쓰기 작업이 완료될 때까지 대기
                List<SandyLogger> loggersToWait;
                lock (_lock)
                {
                    loggersToWait = new List<SandyLogger>(_activeLoggers);
                }

                foreach (var logger in loggersToWait)
                {
                    if (logger._isWriting)
                    {
                        await UniTask.WaitUntil(() => !logger._isWriting);
                    }
                }

                UnityEngine.Debug.Log("Close Done SharedWriter");
                _sharedLogWriter.WriteLine("Close Done SharedWriter");

                _sharedLogWriter.Flush();
                _sharedLogWriter.Close();
                _sharedLogWriter.Dispose();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Failed to close log file: {e.Message}");
            }
            finally
            {
                _sharedLogWriter = null;
                _isInitialized = false;
            }
        }
    }
}