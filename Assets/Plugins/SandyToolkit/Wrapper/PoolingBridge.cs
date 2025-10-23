using SandyCore;
using SandyToolkitCore.Pooling;
using UnityEngine;

public static class PoolingBridge
{
    public static void ClearAllPool()
    {
        ApplicationManager.Instance.PoolingService.ClearAllPool();
    }

    public static void ReturnPoolable(PoolingBase pool)
    {
        ApplicationManager.Instance.PoolingService.ReturnPoolable(pool);
    }

    public static T GetPoolable<T>(string key, Vector3 initPos, int defaultCapacity = 10, int maxSize = 10000) where T : PoolingBase
    {
        return ApplicationManager.Instance.PoolingService.GetPoolable<T>(key, initPos, defaultCapacity, maxSize);
    }
}