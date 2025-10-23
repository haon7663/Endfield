using UnityEngine;
using System;
using System.Threading;

namespace SandyToolkitCore
{
    public struct ControllerInfo
    {
        public string[] ContainSceneNames;
        public int Priority;
        public int UpdateInterval;
        public int LateUpdateInterval;
        public int FixedUpdateInterval;
        public bool IsBackProcess;
    }

    public abstract class BaseController : IDisposable
    {
        protected readonly SandyLogger _logger;

        private static int _nextModuleIndex = 0;

        public bool IsInitialized { get; protected set; } = false;

        protected CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private bool _disposed = false;
        public bool IsDisposed => _disposed;

        public int ModuleIndex { get; private set; }

        public virtual Type ControllerType => GetType();

        public virtual ControllerInfo ControllerInfo { get; } = new()
        {
            ContainSceneNames = new string[] { },
            Priority = 0,
            UpdateInterval = 1,
            LateUpdateInterval = 1,
            FixedUpdateInterval = 1,
            IsBackProcess = false
        };

        public virtual void OnInitialize()
        {
            IsInitialized = true;
        }

        public virtual void OnSceneLoaded(string sceneName)
        {

        }

        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnFixedUpdate() { }
        protected virtual void OnDispose() { }

        public BaseController()
        {
            _logger = new SandyLogger(GetType());
            ModuleIndex = _nextModuleIndex++;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    try
                    {
                        _cancellationTokenSource.Cancel();
                        _cancellationTokenSource.Dispose();

                        OnDispose();

                        _logger.Info($"BaseController::Dispose Complete: {GetType().Name}");
                    }
                    catch (Exception e)
                    {
                        _logger.Error($"BaseController::Dispose Failed: {GetType().Name}, Error: {e.Message}, StackTrace: {e.StackTrace}");
                    }
                    finally
                    {
                        _logger.Dispose();
                    }
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BaseController()
        {
            Dispose(false);
        }
    }
}