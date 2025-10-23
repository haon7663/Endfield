using UnityEditor;
using UnityEngine;

namespace SandyToolkit.Core.Canvas
{
#if UNITY_EDITOR
    [CustomEditor(typeof(CanvasLoader))]
    public class CanvasLoaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            CanvasLoader loader = (CanvasLoader)target;
            EditorGUILayout.Space();

            if (GUILayout.Button("Load Canvas Prefabs"))
            {
                loader.LoadCanvasPrefabs();
            }
        }
    }
#endif
} 