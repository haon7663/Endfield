using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SandyToolkitCore.Pooling;
using SandyToolkitCore.Settings.Interface;
using SandyToolkitCore.Settings.Service;
using UnityEngine;

namespace SandyToolkitCore.Service
{
    public partial interface IPoolingService
    {
        void ClearAllPool();
        void ReturnPoolable(PoolingBase pool);
        T GetPoolable<T>(string key, Vector3 initPos, int defaultCapacity = 10, int maxSize = 10000) where T : PoolingBase;
    }
}