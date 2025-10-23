using Core.Infrastructure.Service;
using UnityEngine;

namespace Core.Infrastructure
{
    public static class GameBootstrapper
    {
        private static bool _isInitialized;
        private static readonly SandyLogger _logger = new(typeof(GameBootstrapper));

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Initialize()
        {
            if (_isInitialized) return;

            _logger.Info("============= Game Bootstrapper Initialize Start =============");

            try
            {
                InitializeGameData();
                InitializeServices();
                InitializeOtherSystems();
            }
            catch (System.Exception e)
            {
                _logger.Error($"Game Bootstrapper Initialize Failed: {e.Message}\n{e.StackTrace}");
            }

            _logger.Info("============= Game Bootstrapper Initialize End =============");
        }

        private static void InitializeGameData()
        {
            _logger.Info("Game Data Loading...");
            
            // 데이터 로드
            
            _logger.Info("Game Data Loading Complete");
        }

        private static void InitializeServices()
        {
            _logger.Info("Service Initialize Start...");

            // ServiceLocator 초기화
            ServiceLocator.Initialize();

            _logger.Info("Service Initialize Complete");
        }

        private static void InitializeOtherSystems()
        {
            
        }

        /// <summary>
        /// 게임 종료 시 정리
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Cleanup()
        {
            if (!_isInitialized) return;

            _logger.Info("Game Bootstrapper Cleanup...");
            ServiceLocator.Dispose();
            _isInitialized = false;
        }
    }
}

