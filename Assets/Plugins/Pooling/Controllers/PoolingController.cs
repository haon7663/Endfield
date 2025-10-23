using System;
using System.Collections.Generic;
using SandyToolkit.Core;
using SandyToolkit.Core.Pooling;
using SandyToolkit.Utility;
using SandyToolkitCore;
using SandyToolkitCore.Pooling;
using SandyToolkitCore.Service;
using UnityEngine;

namespace SandyToolkit.Controllers
{
    public class PoolingController : BaseController, IPoolingService
    {
        public override ControllerInfo ControllerInfo => new()
        {
            ContainSceneNames = new string[] { },
            Priority = 0,
            UpdateInterval = 1,
            LateUpdateInterval = 1,
            FixedUpdateInterval = 1,
            IsBackProcess = true
        };

        private PoolingLoader _poolingLoader;

        private readonly Dictionary<string, PoolablePool> _poolablePools = new();
        public readonly ClassContainer<int, IUpdateLoop> UpdateContainer = new();
        public readonly ClassContainer<int, ILateUpdateLoop> LateUpdateContainer = new();
        public readonly ClassContainer<int, IFixedUpdateLoop> FixedUpdateContainer = new();

        public override void OnInitialize()
        {
            base.OnInitialize();

            // CanvasLoader 로드
            _poolingLoader = Resources.Load<PoolingLoader>("Loader/PoolingLoader");
            if (_poolingLoader == null)
            {
                Debug.LogError("PoolingLoader를 찾을 수 없습니다. Resources/Loader 폴더에 PoolingLoader가 있는지 확인하세요.");
            }
        }

        protected override void OnDispose()
        {
            UpdateContainer.Dispose();
            LateUpdateContainer.Dispose();
            FixedUpdateContainer.Dispose();
            _poolablePools.Clear();
        }

        public void ClearAllPool()
        {
            UpdateContainer.ClearContainer();
            LateUpdateContainer.ClearContainer();
            FixedUpdateContainer.ClearContainer();
            _poolablePools.Clear();
        }

        public override void OnUpdate()
        {
            var validElements = UpdateContainer.GetValidElementsSpan();
            foreach (var loop in validElements)
            {
                loop?.ExecuteUpdate(Time.deltaTime);
            }
        }

        public override void OnFixedUpdate()
        {
            var validElements = FixedUpdateContainer.GetValidElementsSpan();
            foreach (var loop in validElements)
            {
                loop?.ExecuteFixedUpdate(Time.fixedDeltaTime);
            }
        }

        public override void OnLateUpdate()
        {
            var validElements = LateUpdateContainer.GetValidElementsSpan();
            foreach (var loop in validElements)
            {
                loop?.ExecuteLateUpdate(Time.deltaTime);
            }
        }

        public T GetPoolable<T>(string key, Vector3 initPos, int defaultCapacity = 10, int maxSize = 10000) where T : PoolingBase
        {
            var poolablePool = GetPoolablePool(key, defaultCapacity, maxSize);
            var poolingBase = poolablePool.GetPoolable(initPos);

            CachedComponents.TryAddPoolingBase(poolingBase.gameObject, poolingBase);
            foreach (var obj in poolingBase.NeedCachingChildObjects)
            {
                CachedComponents.TryAddPoolingBase(obj, poolingBase);
            }

            AddUpdateObject(poolingBase.gameObject);

            poolingBase.SetPoolKey(key);
            poolingBase.SetSpawned(true);

            var result = UnsafeHelper.UnsafeAs<PoolingBase, T>(poolingBase);
            if (!result)
            {
                _logger.Error($"Failed to cast pooled object to type {typeof(T).Name}\n{poolingBase.gameObject.name}");
                return default;
            }

            return result;
        }

        public void ReturnPoolable(PoolingBase pool)
        {
            CachedComponents.RemovePoolingBase(pool.gameObject);
            foreach (var obj in pool.NeedCachingChildObjects)
            {
                CachedComponents.RemovePoolingBase(obj);
            }

            RemoveUpdateObject(pool.gameObject);

            if (_poolablePools.TryGetValue(pool.PoolKey, out var result).Equals(false))
            {
                _logger.Error($"CannotFind PoolKey : {pool.PoolKey}");
                return;
            }

            pool.SetSpawned(false);
            result.ReturnPoolable(pool);
        }

        private PoolablePool GetPoolablePool(string key, int defaultCapacity = 10, int maxSize = 10000)
        {
            if (_poolablePools.TryGetValue(key, out var result).Equals(false))
            {
                var targetPrefab = _poolingLoader.GetPoolingPrefab(key);
                if (!targetPrefab)
                {
                    _logger.Error($"CannotFind Prefab : {key}");
                    return null;
                }

                result = new PoolablePool();
                result.Initialize(targetPrefab, defaultCapacity, maxSize);
                _poolablePools.Add(key, result);
            }

            return result;
        }

        private void AddUpdateObject(GameObject gameObj)
        {
            var instanceId = gameObj.GetInstanceID();

            IUpdateLoop updateLoop = CachedComponents.GetUpdateLoop(gameObj);
            if (updateLoop != null)
            {
                AddUpdateObject(instanceId, updateLoop);
            }

            ILateUpdateLoop lateUpdateLoop = CachedComponents.GetLateUpdateLoop(gameObj);
            if (lateUpdateLoop != null)
            {
                AddLateUpdateObject(instanceId, lateUpdateLoop);
            }

            IFixedUpdateLoop fixedUpdateLoop = CachedComponents.GetFixedUpdateLoop(gameObj);
            if (fixedUpdateLoop != null)
            {
                AddFixedUpdateObject(instanceId, fixedUpdateLoop);
            }
        }

        private void RemoveUpdateObject(GameObject gameObj)
        {
            var instanceId = gameObj.GetInstanceID();

            IUpdateLoop loop = CachedComponents.GetUpdateLoop(gameObj);
            if (loop != null)
            {
                RemoveUpdateObject(instanceId);
            }

            ILateUpdateLoop lateLoop = CachedComponents.GetLateUpdateLoop(gameObj);
            if (lateLoop != null)
            {
                RemoveLateUpdateObject(instanceId);
            }

            IFixedUpdateLoop fixedLoop = CachedComponents.GetFixedUpdateLoop(gameObj);
            if (fixedLoop != null)
            {
                RemoveFixedUpdateObject(instanceId);
            }
        }

        private void AddUpdateObject(int id, IUpdateLoop loop)
        {
            UpdateContainer.AddElement(id, loop);
        }

        private void AddLateUpdateObject(int id, ILateUpdateLoop loop)
        {
            LateUpdateContainer.AddElement(id, loop);
        }

        private void AddFixedUpdateObject(int id, IFixedUpdateLoop loop)
        {
            FixedUpdateContainer.AddElement(id, loop);
        }

        private void RemoveUpdateObject(int id)
        {
            UpdateContainer.RemoveElement(id);
        }

        private void RemoveLateUpdateObject(int id)
        {
            LateUpdateContainer.RemoveElement(id);
        }

        private void RemoveFixedUpdateObject(int id)
        {
            FixedUpdateContainer.RemoveElement(id);
        }
    }

    [Serializable]
    public class KeyValuePoolObject
    {
        public string Key;
        public PoolingBase Value;
    }


    [Serializable]
    public class KeyValuePoolTexture
    {
        public string Key;
        public Texture2D Value;
    }
}