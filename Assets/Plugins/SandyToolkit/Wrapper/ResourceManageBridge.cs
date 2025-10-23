using SandyCore;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System;

public static class ResourceManageBridge
{
    public static async UniTask SceneResourceChange(string oldSceneName, string newSceneName)
    {
        await ApplicationManager.Instance.ResourceManageService.SceneResourceChange(oldSceneName, newSceneName);
    }
    public static T GetLoadedAsset<T>(string address) where T : UnityEngine.Object
    {
        return ApplicationManager.Instance.ResourceManageService.GetLoadedAsset<T>(address);
    }
    public static List<T> GetLoadedAssetsByType<T>() where T : UnityEngine.Object
    {
        return ApplicationManager.Instance.ResourceManageService.GetLoadedAssetsByType<T>();
    }
    public static List<string> GetGroupAddresses(string groupName)
    {
        return ApplicationManager.Instance.ResourceManageService.GetGroupAddresses(groupName);
    }
    public static async UniTask<T> LoadAssetAsync<T>(string address) where T : UnityEngine.Object
    {
        return await ApplicationManager.Instance.ResourceManageService.LoadAssetAsync<T>(address);
    }
    public static async UniTask LoadGroupAsync(string groupName, Action<float> progressCallback = null)
    {
        await ApplicationManager.Instance.ResourceManageService.LoadGroupAsync(groupName, progressCallback);
    }
    public static async UniTask UnloadGroupAsync(string groupName)
    {
        await ApplicationManager.Instance.ResourceManageService.UnloadGroupAsync(groupName);
    }
    public static async UniTask CleanupMemory()
    {
        await ApplicationManager.Instance.ResourceManageService.CleanupMemory();
    }
}
