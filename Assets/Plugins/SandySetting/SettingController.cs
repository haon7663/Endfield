using System;
using Cysharp.Threading.Tasks;
using SandySetting.Service;
using SandyToolkitCore;
using SandyToolkitCore.Service;
using SandyToolkitCore.Settings.Interface;
using SandyToolkitCore.Settings.Service;

namespace SandySetting
{
    public abstract partial class SettingController : BaseController, ISettingService
    {
        public override ControllerInfo ControllerInfo => new()
        {
            ContainSceneNames = new string[] { },
            Priority = 0,
            UpdateInterval = 0,
            LateUpdateInterval = 0,
            FixedUpdateInterval = 0,
            IsBackProcess = true
        };

        private SettingRepositoryService _settingRepositoryService = new SettingRepositoryService();

        #region ISettingService Implement   
        private AudioSettingService _audioSettingService;
        public IAudioSettingService AudioSetting => _audioSettingService;

        private GameSettingService _gameSettingService;
        public IGameSettingService GameSetting => _gameSettingService;
        #endregion

        public IGameSettings GameSettings { get; private set; }

        public IAudioSetting AudioSettings => GameSettings.Audio;


        public override void OnInitialize()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            if (IsInitialized) return;

            InitializeGameAsync().Forget();
        }

        protected void SaveSettings(IGameSettings settings)
        {
            _settingRepositoryService.SaveSettings(settings);
        }

        protected virtual void OnInitializeGame()
        {

        }

        private async UniTaskVoid InitializeGameAsync()
        {
            _logger.Info("SettingController::InitializeGameAsync Start");

            try
            {
                GameSettings = await _settingRepositoryService.LoadAsync(_cancellationTokenSource.Token);

                _gameSettingService = new GameSettingService(GameSettings);
                _gameSettingService.OnLanguageChanged += (language) =>
                {
                    SaveSettings(GameSettings);
                };

                _audioSettingService = new AudioSettingService(AudioSettings);
                _audioSettingService.OnMasterVolumeChanged += () =>
                {
                    SaveSettings(GameSettings);
                };
                _audioSettingService.OnBgmVolumeChanged += () =>
                {
                    SaveSettings(GameSettings);
                };
                _audioSettingService.OnSfxVolumeChanged += () =>
                {
                    SaveSettings(GameSettings);
                };
                _audioSettingService.OnAmbienceVolumeChanged += () =>
                {
                    SaveSettings(GameSettings);
                };

                OnInitializeGame();

                _logger.Info("SettingController::InitializeGameAsync Complete");
            }
            catch (OperationCanceledException)
            {
                _logger.Warning("InitializeGameAsync Canceled");
                return;
            }
            catch (Exception e)
            {
                _logger.Error("Game Initialize Failed");
                _logger.Error(e.Message);
                _logger.Error(e.StackTrace);
                return;
            }

            IsInitialized = true;
        }
    }
}