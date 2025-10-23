using System;
using System.Collections.Generic;
using System.Linq;
using SandyToolkit.Core;
using SandyToolkit.Utility;
using SandyToolkitCore.Pooling;
using UnityEngine;

namespace SandyToolkit.Utility
{
    public static class CachedComponents
    {
        // 캐시 크기 제한
        private const int MaxCacheSize = 1000;

        // 스레드 안전성을 위한 잠금 객체
        private static readonly object _lockObject = new object();

        // 다양한 타입의 캐시 딕셔너리
        private static Dictionary<int, PoolingBase> _cachedPoolingBases = new();
        private static Dictionary<int, IUpdateLoop> _cachedIUpdateLoop = new();
        private static Dictionary<int, ILateUpdateLoop> _cachedILateUpdateLoop = new();
        private static Dictionary<int, IFixedUpdateLoop> _cachedIFixedUpdateLoop = new();

        // 제네릭 캐싱 메서드
        private static T GetOrAddComponent<T>(GameObject obj, Dictionary<int, T> cache) where T : class
        {
            if (obj == null)
            {
                Debug.LogWarning($"Attempted to get component {typeof(T).Name} from null GameObject");
                return null;
            }

            int instanceId = obj.GetInstanceID();

            lock (_lockObject)
            {
                // 캐시에서 먼저 찾기
                if (cache.TryGetValue(instanceId, out T cachedComponent))
                {
                    return cachedComponent;
                }

                // 컴포넌트 찾기
                T component = obj.GetComponent<T>();
                if (component != null)
                {
                    // 캐시 크기 제한 확인
                    EnsureCacheLimit(cache);
                    cache[instanceId] = component;
                }

                return component;
            }
        }

        // 캐시 크기 제한 메서드
        private static void EnsureCacheLimit<T>(Dictionary<int, T> cache)
        {
            if (cache.Count >= MaxCacheSize)
            {
                // 가장 오래된 항목 제거 (가장 작은 인스턴스 ID)
                var oldestKey = cache.Keys.Min();
                cache.Remove(oldestKey);
            }
        }

        // 전체 캐시 초기화
        public static void Reset()
        {
            lock (_lockObject)
            {
                _cachedPoolingBases.Clear();
                _cachedIUpdateLoop.Clear();
                _cachedILateUpdateLoop.Clear();
                _cachedIFixedUpdateLoop.Clear();
            }
        }

        // 특정 타입의 컴포넌트 추가 메서드들
        public static bool TryAddPoolingBase(GameObject obj, PoolingBase poolingBase)
        {
            lock (_lockObject)
            {
                int instanceId = obj.GetInstanceID();
                if (!_cachedPoolingBases.TryAdd(instanceId, poolingBase)) return false;

                return true;
            }
        }

        // 컴포넌트 제거 메서드들
        public static void RemovePoolingBase(GameObject obj)
        {
            lock (_lockObject)
            {
                int instanceId = obj.GetInstanceID();
                _cachedPoolingBases.Remove(instanceId);
            }
        }

        // 주요 Get 메서드들 (제네릭 메서드 활용)
        public static PoolingBase GetPoolingBase(GameObject obj)
        {
            return GetOrAddComponent<PoolingBase>(obj, _cachedPoolingBases);
        }

        // 디버깅 및 모니터링 메서드
        public static Dictionary<Type, int> GetCacheSizeByType()
        {
            return new Dictionary<Type, int>
                {
                    { typeof(PoolingBase), _cachedPoolingBases.Count },
                };
        }

        public static IUpdateLoop GetUpdateLoop(GameObject obj)
        {
            int instanceId = obj.GetInstanceID();

            if (_cachedIUpdateLoop.TryGetValueIL2CPP(instanceId, out var updateLoop))
            {
                return updateLoop;
            }
            else if (obj.TryGetComponent(out updateLoop))
            {
                _cachedIUpdateLoop.Add(instanceId, updateLoop);
                return updateLoop;
            }

            return updateLoop;
        }

        public static ILateUpdateLoop GetLateUpdateLoop(GameObject obj)
        {
            int instanceId = obj.GetInstanceID();

            if (_cachedILateUpdateLoop.TryGetValueIL2CPP(instanceId, out var lateUpdateLoop))
            {
                return lateUpdateLoop;
            }
            else if (obj.TryGetComponent<ILateUpdateLoop>(out lateUpdateLoop))
            {
                _cachedILateUpdateLoop.Add(instanceId, lateUpdateLoop);
                return lateUpdateLoop;
            }

            return lateUpdateLoop;
        }

        public static IFixedUpdateLoop GetFixedUpdateLoop(GameObject obj)
        {
            int instanceId = obj.GetInstanceID();

            if (_cachedIFixedUpdateLoop.TryGetValueIL2CPP(instanceId, out var fixedUpdateLoop))
            {
                return fixedUpdateLoop;
            }
            else if (obj.TryGetComponent<IFixedUpdateLoop>(out fixedUpdateLoop))
            {
                _cachedIFixedUpdateLoop.Add(instanceId, fixedUpdateLoop);
                return fixedUpdateLoop;
            }

            return fixedUpdateLoop;
        }
    }

    // 로깅 유틸리티 (선택적)
    public static class CachedComponentsLogger
    {
        public static void LogCacheStats()
        {
            var stats = CachedComponents.GetCacheSizeByType();
            foreach (var stat in stats)
            {
                Debug.Log($"{stat.Key.Name}: {stat.Value} cached components");
            }
        }
    }
}