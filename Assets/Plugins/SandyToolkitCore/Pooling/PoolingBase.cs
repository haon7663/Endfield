using System;
using UnityEngine;

namespace SandyToolkitCore.Pooling
{
    [Serializable]
    public abstract class PoolingBase : MonoBehaviour
    {
        public GameObject[] NeedCachingChildObjects;

        public bool IsSpawned { get; private set; }

        public void SetSpawned(bool isSpawn)
        {
            IsSpawned = isSpawn;
        }

        public string PoolKey { get; private set; }

        public void SetPoolKey(string key)
        {
            PoolKey = key;
        }

        public virtual void OnInstantiate()
        {
        }

        public virtual void OnSpawn()
        {
        }

        public virtual void OnDespawn()
        {
        }

        public virtual void Release()
        {
        }

        public virtual void ReturnDelay(PoolingBase pool, Action onReturnComplete = null)
        {
        }
    }
}