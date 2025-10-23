using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System;
using System.Threading;
using SandyToolkitCore.Settings.Interface;
using SandySetting.Datas;

namespace SandySetting.Service
{
    public class SettingRepositoryService
    {
        private const string SupportedVersion = "0.1.0";
        private const string DefaultPath = "settings.json";

        private readonly string _configPath;

        public SettingRepositoryService()
        {
            _configPath = Path.Combine(Application.persistentDataPath, DefaultPath);

            Directory.CreateDirectory(Path.GetDirectoryName(_configPath)!);
        }

        public async UniTask<IGameSettings> LoadAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    var defaultSettings = CreateDefaultSettings();
                    await CreateTextFileAsync(defaultSettings, cancellationToken);
                    return defaultSettings;
                }

                var json = await File.ReadAllTextAsync(_configPath, cancellationToken);
                var settings = JsonConvert.DeserializeObject<GameSettings>(json);

                if (settings is not { Version: SupportedVersion })
                {
                    Debug.Log($"Settings version not matched. Old: {settings.Version}, Current: {SupportedVersion}");

                    // 백업 생성
                    var backupPath = _configPath + $"{settings?.Version}.backup";
                    File.WriteAllText(backupPath, json);

                    var newSettings = CreateDefaultSettings();
                    await CreateTextFileAsync(newSettings, cancellationToken);
                    return newSettings;
                }

                return settings;
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("설정 불러오기 작업이 취소되었습니다.");
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"설정 불러오기 실패: {e}");
                throw;
            }
        }

        public async UniTask<bool> SaveAsync(IGameSettings settings, CancellationToken cancellationToken)
        {
            try
            {
                await CreateTextFileAsync(settings, cancellationToken);
                return true;
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("설정 저장 작업이 취소되었습니다.");
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"설정 저장 실패: {e}");
                throw;
            }
        }

        public bool SaveSettings(IGameSettings settings)
        {
            try
            {
                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(_configPath, json);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"설정 저장 실패: {e.Message}");
                throw;
            }
        }

        public IGameSettings LoadSettings()
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    var defaultSettings = CreateDefaultSettings();
                    CreateTextFile(defaultSettings);
                    return defaultSettings;
                }

                var json = File.ReadAllText(_configPath);
                var settings = JsonConvert.DeserializeObject<IGameSettings>(json);

                if (settings is not { Version: SupportedVersion })
                {
                    Debug.Log($"Settings version not matched. Old: {settings.Version}, Current: {SupportedVersion}");

                    // 백업 생성
                    var backupPath = _configPath + $"{settings?.Version}.backup";
                    File.WriteAllText(backupPath, json);

                    var newSettings = CreateDefaultSettings();
                    CreateTextFile(newSettings);
                    return newSettings;
                }

                return settings;
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("설정 불러오기 작업이 취소되었습니다.");
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"설정 불러오기 실패: {e}");
                throw;
            }
        }

        private async UniTask CreateTextFileAsync(IGameSettings settings, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            await File.WriteAllTextAsync(_configPath, json, cancellationToken);
        }

        private void CreateTextFile(IGameSettings settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(_configPath, json);
        }


        private IGameSettings CreateDefaultSettings()
        {
            var settings = new GameSettings
            {
                Version = SupportedVersion
            };
            return settings;
        }
    }
}