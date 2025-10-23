using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace SandyToolkit.Core.Canvas
{
    [CreateAssetMenu(fileName = "CanvasLoader", menuName = "Sandy/Loader/CanvasLoader")]
    public class CanvasLoader : ScriptableObject
    {
        [Serializable]
        public class CanvasInfo
        {
            public string Key;
            public GameObject Prefab;
            public string SceneName;
        }

        public List<CanvasInfo> CanvasDictionary = new List<CanvasInfo>();

#if UNITY_EDITOR
        public void LoadCanvasPrefabs()
        {
            CanvasDictionary.Clear();
            
            // 모든 폴더에서 ViewCanvas를 상속받은 프리팹을 찾습니다
            string[] guids = AssetDatabase.FindAssets("t:Prefab");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                
                if (prefab != null)
                {
                    var viewCanvas = prefab.GetComponent<ViewCanvas>();
                    if (viewCanvas != null)
                    {
                        var canvasInfo = new CanvasInfo
                        {
                            Key = prefab.name,
                            Prefab = prefab,
                            SceneName = GetSceneNameFromPrefab(prefab)
                        };
                        CanvasDictionary.Add(canvasInfo);
                    }
                }
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            Debug.Log($"총 {CanvasDictionary.Count}개의 Canvas 프리팹이 로드되었습니다.");
        }

        private string GetSceneNameFromPrefab(GameObject prefab)
        {
            // 프리팹의 이름이나 태그에서 씬 정보를 추출하는 로직
            // 예: "MainMenu_Canvas" -> "MainMenu"
            return prefab.name.Split('_')[0];
        }
#endif

        public GameObject GetCanvasPrefab(string key)
        {
            return CanvasDictionary.FirstOrDefault(x => x.Key == key)?.Prefab;
        }

        public List<CanvasInfo> GetCanvasesForScene(string sceneName)
        {
            return CanvasDictionary.Where(x => x.SceneName == sceneName).ToList();
        }
    }
} 