using UnityEditor;
using UnityEngine;

namespace SandyToolkit.Core.Pooling
{
#if UNITY_EDITOR
    [CustomEditor(typeof(PoolingLoader))]
    public class PoolingLoaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PoolingLoader loader = (PoolingLoader)target;
            EditorGUILayout.Space();

            if (GUILayout.Button("Load Pooling Prefabs"))
            {
                loader.LoadPoolingPrefabs();
            }
        }
    }
#endif
} 