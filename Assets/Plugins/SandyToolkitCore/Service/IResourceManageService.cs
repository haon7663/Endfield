using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SandyToolkitCore.Settings.Interface;
using SandyToolkitCore.Settings.Service;

namespace SandyToolkitCore.Service
{
    public partial interface IResourceManageService
    {
        UniTask SceneResourceChange(string oldSceneName, string newSceneName);
        T GetLoadedAsset<T>(string address) where T : UnityEngine.Object;
        List<T> GetLoadedAssetsByType<T>() where T : UnityEngine.Object;
        List<string> GetGroupAddresses(string groupName);
        UniTask<T> LoadAssetAsync<T>(string address) where T : UnityEngine.Object;
        UniTask LoadGroupAsync(string groupName, Action<float> progressCallback = null);
        UniTask UnloadGroupAsync(string groupName);
        UniTask CleanupMemory();
    }
}