using System;
using System.Collections.Generic;

namespace Core.Infrastructure.Service
{
    /// <summary>
    /// 서비스 관리자 - 모든 서비스의 생명주기를 관리
    /// </summary>
    public class ServiceManager : IDisposable
    {
        private static ServiceManager _instance;
        public static ServiceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceManager();
                }
                return _instance;
            }
        }

        private readonly Dictionary<Type, object> _services = new();
        private readonly SandyLogger _logger = new(typeof(ServiceManager));
        private bool _isInitialized;

        private ServiceManager() { }

        /// <summary>
        /// 서비스 초기화
        /// </summary>
        public void Initialize()
        {
            if (_isInitialized) return;

            _logger.Info("서비스 매니저 초기화 시작");

            // Repository 등록
            RegisterRepository();

            // 서비스 등록
            RegisterServices();

            _isInitialized = true;
            _logger.Info("서비스 매니저 초기화 완료");
        }

        private void RegisterRepository()
        {
        }

        private void RegisterServices()
        {
        }

        /// <summary>
        /// 서비스 등록
        /// </summary>
        public void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            
            if (_services.ContainsKey(type))
            {
                _logger.Warning($"서비스 {type.Name}이 이미 등록되어 있습니다. 덮어씁니다.");
            }

            _services[type] = service;
            _logger.Info($"서비스 등록됨: {type.Name}");
        }

        /// <summary>
        /// 서비스 가져오기
        /// </summary>
        public T Get<T>() where T : class
        {
            var type = typeof(T);
            
            if (_services.TryGetValue(type, out var service))
            {
                return service as T;
            }

            _logger.Error($"서비스 {type.Name}을 찾을 수 없습니다.");
            return null;
        }

        /// <summary>
        /// 서비스 존재 여부 확인
        /// </summary>
        public bool Has<T>() where T : class
        {
            return _services.ContainsKey(typeof(T));
        }

        /// <summary>
        /// 모든 서비스 정리
        /// </summary>
        public void Dispose()
        {
            _logger.Info("서비스 매니저 정리 시작");

            foreach (var service in _services.Values)
            {
                if (service is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _services.Clear();
            _isInitialized = false;
            _instance = null;

            _logger.Info("서비스 매니저 정리 완료");
        }

        /// <summary>
        /// 서비스 상태 출력 (디버그용)
        /// </summary>
        public void PrintServiceStatus()
        {
            _logger.Info("=== 등록된 서비스 목록 ===");
            foreach (var kvp in _services)
            {
                _logger.Info($"- {kvp.Key.Name}: {kvp.Value.GetType().Name}");
            }
            _logger.Info($"총 {_services.Count}개 서비스 등록됨");
        }
    }

    /// <summary>
    /// 서비스 로케이터 - 간편한 서비스 접근
    /// </summary>
    public static class ServiceLocator
    {
        private static ServiceManager Manager => ServiceManager.Instance;

        public static void Initialize()
        {
            Manager.Initialize();
        }

        public static T Get<T>() where T : class
        {
            return Manager.Get<T>();
        }

        public static bool Has<T>() where T : class
        {
            return Manager.Has<T>();
        }

        public static void Dispose()
        {
            Manager.Dispose();
        }
    }
}