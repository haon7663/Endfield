using UnityEditor;
using UnityEngine;

namespace SandyToolkit.Core.UI
{
#if UNITY_EDITOR
    [CustomEditor(typeof(UIDocumentLoader))]
    public class UIDocumentLoaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UIDocumentLoader loader = (UIDocumentLoader)target;
            EditorGUILayout.Space();

            if (GUILayout.Button("Load UIDocument Prefabs"))
            {
                loader.LoadUIDocumentPrefabs();
            }
        }
    }
#endif
} 