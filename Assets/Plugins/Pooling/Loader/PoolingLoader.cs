using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using SandyToolkitCore.Pooling;

namespace SandyToolkit.Core.Pooling
{
    [CreateAssetMenu(fileName = "PoolingLoader", menuName = "Sandy/Loader/PoolingLoader")]
    public class PoolingLoader : ScriptableObject
    {
        [Serializable]
        public class PoolingInfo
        {
            public string Key;
            public PoolingBase Prefab;
        }

        public List<PoolingInfo> PoolingDictionary = new List<PoolingInfo>();

#if UNITY_EDITOR
        public void LoadPoolingPrefabs()
        {
            PoolingDictionary.Clear();
            
            // 모든 폴더에서 ViewCanvas를 상속받은 프리팹을 찾습니다
            string[] guids = AssetDatabase.FindAssets("t:Prefab");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                
                if (prefab != null)
                {
                    var poolingBase = prefab.GetComponent<PoolingBase>();
                    if (poolingBase != null)
                    {
                        var poolingInfo = new PoolingInfo
                        {
                            Key = prefab.name,
                            Prefab = poolingBase,
                        };
                        PoolingDictionary.Add(poolingInfo);
                    }
                }
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            Debug.Log($"총 {PoolingDictionary.Count}개의 Pooling 프리팹이 로드되었습니다.");
        }
#endif

        public PoolingBase GetPoolingPrefab(string key)
        {
            return PoolingDictionary.FirstOrDefault(x => x.Key == key)?.Prefab;
        }
    }
} 