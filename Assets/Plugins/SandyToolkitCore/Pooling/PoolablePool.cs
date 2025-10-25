using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace SandyToolkitCore.Pooling
{
    public class PoolablePool
    {
        private PoolingBase _targetPoolablePrefab;
        private ObjectPool<PoolingBase> _poolingBasePool;
        private Vector3 _initPos;

        public void Initialize(PoolingBase poolablePrefab, int defaultCapacity, int maxSize)
        {
            _targetPoolablePrefab = poolablePrefab;
            _poolingBasePool = new ObjectPool<PoolingBase>(
                CreatePoolable,
                OnTakeFromPool,
                OnReturnToPool,
                OnDestroyPoolObject,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );
        }

        public PoolingBase GetPoolable(Vector3 initPos)
        {
            _initPos = initPos;
            var getPoolable = _poolingBasePool.Get();
            getPoolable.transform.position = initPos;
            return getPoolable;
        }

        public void ReturnPoolable(PoolingBase poolingBase)
        {
            try
            {
                _poolingBasePool.Release(poolingBase);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"PoolablePool ReturnPoolable Error: {e}");
            }
        }

        private PoolingBase CreatePoolable()
        {
            var poolable = Object.Instantiate(_targetPoolablePrefab, _initPos, Quaternion.identity);
            poolable.OnInstantiate();
            return poolable;
        }

        private void OnTakeFromPool(PoolingBase poolable)
        {
            poolable.transform.position = _initPos;
            poolable.gameObject.SetActive(true);
            poolable.OnSpawn();
        }

        private void OnReturnToPool(PoolingBase poolable)
        {
            poolable.gameObject.SetActive(false);
            poolable.OnDespawn();
        }

        private void OnDestroyPoolObject(PoolingBase poolable)
        {
            poolable.Release();
            Object.Destroy(poolable.gameObject);
        }
    }
}