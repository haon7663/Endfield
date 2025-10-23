using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace SandyToolkit.Core.UI
{
    [CreateAssetMenu(fileName = "UIDocumentLoader", menuName = "Sandy/Loader/UIDocumentLoader")]
    public class UIDocumentLoader : ScriptableObject
    {
        [Serializable]
        public class UIDocumentInfo
        {
            public string Key;
            public GameObject Prefab;
            public string SceneName;
        }

        public List<UIDocumentInfo> UIDocumentDictionary = new List<UIDocumentInfo>();

#if UNITY_EDITOR
        public void LoadUIDocumentPrefabs()
        {
            UIDocumentDictionary.Clear();
            
            // Resources 폴더에서 UIDocumentView를 상속받은 모든 프리팹을 찾습니다
            var prefabs = Resources.LoadAll<GameObject>("");
            foreach (var prefab in prefabs)
            {
                var uiDocumentView = prefab.GetComponent<UIDocumentView>();
                if (uiDocumentView != null)
                {
                    var documentInfo = new UIDocumentInfo
                    {
                        Key = prefab.name,
                        Prefab = prefab,
                        SceneName = GetSceneNameFromPrefab(prefab)
                    };
                    UIDocumentDictionary.Add(documentInfo);
                }
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            Debug.Log($"총 {UIDocumentDictionary.Count}개의 UIDocument 프리팹이 로드되었습니다.");
        }

        private string GetSceneNameFromPrefab(GameObject prefab)
        {
            return prefab.name.Split('_')[0];
        }
#endif

        public GameObject GetUIDocumentPrefab(string key)
        {
            return UIDocumentDictionary.FirstOrDefault(x => x.Key == key)?.Prefab;
        }

        public List<UIDocumentInfo> GetUIDocumentsForScene(string sceneName)
        {
            return UIDocumentDictionary.Where(x => x.SceneName == sceneName).ToList();
        }
    }
} 