using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SandyAddressable.Data
{
    public class TypedCache<T> where T : UnityEngine.Object
    {
        private readonly Dictionary<string, T> assets = new Dictionary<string, T>();
        private readonly Dictionary<string, AsyncOperationHandle<T>> handles = new Dictionary<string, AsyncOperationHandle<T>>();

        public bool HasAsset(string key) => assets.ContainsKey(key);
        public T GetAsset(string key) => assets.GetValueOrDefault(key);

        public void AddAsset(string key, T asset, AsyncOperationHandle<T> handle)
        {
            assets[key] = asset;
            handles[key] = handle;
        }

        public void RemoveAsset(string key)
        {
            if (handles.TryGetValue(key, out var handle))
            {
                Addressables.Release(handle);
                handles.Remove(key);
            }

            assets.Remove(key);
        }
        
        public List<T> GetAllAssets() => assets.Values.ToList();

        public void Clear()
        {
            foreach (var handle in handles.Values)
            {
                Addressables.Release(handle);
            }

            handles.Clear();
            assets.Clear();
        }
    }
}