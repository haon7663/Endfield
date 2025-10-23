using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SandyAddressable.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using SandyToolkitCore;
using SandyToolkitCore.Service;

#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
#endif

namespace SandyAddressable.Core
{
    public class AddressableSystem : BaseController, IResourceManageService
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

        private AddressableSystemConfig config;
        private readonly Dictionary<Type, object> typedCaches = new Dictionary<Type, object>();
        private readonly Dictionary<string, int> groupRefCounts = new Dictionary<string, int>();

        public override void OnInitialize()
        {
            LoadConfig();
            InitializeAddressablesAsync().Forget();
        }

        private void LoadConfig()
        {
            config = Resources.Load<AddressableSystemConfig>(AddressableSystemConfig.RESOURCES_PATH);
            if (config == null)
            {
                _logger.Error($"AddressableSystemConfig를 찾을 수 없습니다.\n다음 경로에 설정 파일을 생성해주세요: Resources/{AddressableSystemConfig.RESOURCES_PATH}.asset\n생성 방법:\n1. Create > Sandy > Addressable System Config 선택\n2. 생성된 파일을 Resources/Addressable 폴더에 저장");
                return;
            }
        }

        protected override void OnDispose()
        {
            UnloadAllAssets();
            Resources.UnloadUnusedAssets();
        }

        // 또는 주기적 정리
        public async UniTask CleanupMemory()
        {
            var operation = Resources.UnloadUnusedAssets();
            while (!operation.isDone)
            {
                await UniTask.Yield();
            }
        }

        // Addressables 시스템 초기화
        private async UniTask InitializeAddressablesAsync()
        {
            try
            {
                // 기존의 모든 Addressables 캐시 클리어
                await Addressables.CleanBundleCache();
                await CleanupMemory();

                _logger.Debug("Addressables system initialized");

                InitializeGroupRefCounts();
            }
            catch (System.Exception e)
            {
                _logger.Error($"Failed to initialize Addressables: {e.Message}");
            }
            finally
            {
                IsInitialized = true;
            }
        }


        private TypedCache<T> GetCache<T>() where T : UnityEngine.Object
        {
            var type = typeof(T);
            if (!typedCaches.TryGetValue(type, out var cache))
            {
                cache = new TypedCache<T>();
                typedCaches[type] = cache;
            }

            return (TypedCache<T>)cache;
        }

        private void InitializeGroupRefCounts()
        {
            foreach (var group in config.GroupData)
            {
                groupRefCounts[group.groupName] = 0;
            }
        }

        public async UniTask LoadGroupAsync(string groupName, Action<float> progressCallback = null)
        {
            var group = config.GroupData.Find(g => g.groupName == groupName);
            if (group == null)
            {
                _logger.Error($"Group {groupName} not found!");
                return;
            }

            groupRefCounts.TryAdd(groupName, 0);

            groupRefCounts[groupName]++;
            if (groupRefCounts[groupName] > 1) return; // 이미 로드된 그룹

            int totalAssets = group.assetAddresses.Count;
            int loadedAssets = 0;

            foreach (string address in group.assetAddresses)
            {
                try
                {
                    // 에셋 타입 확인을 위한 리소스 로케이션 가져오기
                    var locationsHandle = Addressables.LoadResourceLocationsAsync(address);
                    var locations = await locationsHandle.ToUniTask();

                    foreach (var location in locations)
                    {
                        Type assetType = location.Data is Type
                            ? (Type)location.Data
                            : location.ResourceType ?? typeof(UnityEngine.Object);

                        if (assetType == typeof(Sprite))
                            await LoadTypedAsset<Sprite>(address, loadedAssets, totalAssets, progressCallback);
                        else if (assetType == typeof(GameObject))
                            await LoadTypedAsset<GameObject>(address, loadedAssets, totalAssets, progressCallback);
                        else if (assetType == typeof(TextAsset))
                            await LoadTypedAsset<TextAsset>(address, loadedAssets, totalAssets, progressCallback);
                        else if (assetType == typeof(AudioClip))
                            await LoadTypedAsset<AudioClip>(address, loadedAssets, totalAssets, progressCallback);
                        else if (assetType.IsSubclassOf(typeof(ScriptableObject)))
                            await LoadTypedAsset<ScriptableObject>(address, loadedAssets, totalAssets, progressCallback);
                        else
                            await LoadTypedAsset<UnityEngine.Object>(address, loadedAssets, totalAssets, progressCallback);
                    }

                    Addressables.Release(locationsHandle);
                    loadedAssets++;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to load asset {address}: {e.Message}");
                }
            }
        }

        private async UniTask LoadTypedAsset<T>(
            string address,
            int loadedAssets,
            int totalAssets,
            Action<float> progressCallback) where T : UnityEngine.Object
        {
            var handle = Addressables.LoadAssetAsync<T>(address);
            var cache = GetCache<T>();

            handle.Completed += (op) =>
            {
                float progress = (float)loadedAssets / totalAssets;
                progressCallback?.Invoke(progress);

                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    cache.AddAsset(address, op.Result, handle);
                }
            };

            await handle.ToUniTask();
        }

        public async UniTask UnloadGroupAsync(string groupName)
        {
            var group = config.GroupData.Find(g => g.groupName == groupName);
            if (group == null) return;

            if (!groupRefCounts.ContainsKey(groupName)) return;

            groupRefCounts[groupName]--;
            if (groupRefCounts[groupName] > 0) return; // 아직 다른 곳에서 사용 중

            foreach (string address in group.assetAddresses)
            {
                foreach (var cache in typedCaches.Values)
                {
                    var methodInfo = cache.GetType().GetMethod("RemoveAsset");
                    methodInfo?.Invoke(cache, new object[] { address });
                }
            }
        }

        public async UniTask SceneResourceChange(string oldSceneName, string newSceneName)
        {
            string previousSceneName = oldSceneName;

            if (!string.IsNullOrEmpty(previousSceneName))
            {
                var previousConfig = config.SceneConfigs.Find(g => g.sceneName == previousSceneName);
                if (previousConfig != null)
                {
                    foreach (string groupName in previousConfig.groupNames)
                    {
                        await UnloadGroupAsync(groupName);
                    }
                }
            }

            await CleanupMemory();

            var newConfig = config.SceneConfigs.Find(g => g.sceneName == newSceneName);
            if (newConfig != null)
            {
                foreach (string groupName in newConfig.groupNames)
                {
                    await LoadGroupAsync(groupName);
                }
            }
        }

        // 이미 로드된 에셋 가져오기
        public T GetLoadedAsset<T>(string address) where T : UnityEngine.Object
        {
            var cache = GetCache<T>();
            return cache.GetAsset(address);
        }

        public List<T> GetLoadedAssetsByType<T>() where T : UnityEngine.Object
        {
            var cache = GetCache<T>();
            return cache.GetAllAssets();
        }

        // 그룹에 있는 주소를 다 준다.
        public List<string> GetGroupAddresses(string groupName)
        {
            var group = config.GroupData.Find(g => g.groupName == groupName);
            if (group == null) return null;

            return group.assetAddresses;
        }

        // 새로운 에셋 로드
        public async UniTask<T> LoadAssetAsync<T>(string address) where T : UnityEngine.Object
        {
            var cache = GetCache<T>();

            if (cache.HasAsset(address))
            {
                return cache.GetAsset(address);
            }

            try
            {
                var handle = Addressables.LoadAssetAsync<T>(address);
                T result = await handle.ToUniTask();

                if (result != null)
                {
                    cache.AddAsset(address, result, handle);
                }

                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load asset {address}: {e.Message}");
                return null;
            }
        }

        private void UnloadAllAssets()
        {
            foreach (var cache in typedCaches.Values)
            {
                var methodInfo = cache.GetType().GetMethod("Clear");
                methodInfo?.Invoke(cache, null);
            }

            typedCaches.Clear();

            foreach (var groupName in groupRefCounts.Keys.ToList())
            {
                groupRefCounts[groupName] = 0;
            }

        }
    }
}