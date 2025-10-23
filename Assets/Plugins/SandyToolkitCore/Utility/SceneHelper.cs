#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SceneHelper
{
    public static void AddSceneToBuildSettings(string scenePath)
    {
        // 현재 Build Settings의 Scene 목록 가져오기
        var scenes = EditorBuildSettings.scenes.ToList();

        // 이미 추가되어 있는지 확인
        if (!scenes.Any(s => s.path == scenePath))
        {
            // 새로운 Scene 추가
            var newScene = new EditorBuildSettingsScene(scenePath, true);
            scenes.Add(newScene);

            // Build Settings 업데이트
            EditorBuildSettings.scenes = scenes.ToArray();
            Debug.Log($"Scene이 Build Settings에 추가되었습니다: {scenePath}");
        }

        Debug.Log($"Scene이 이미 Build Settings에 있습니다: {scenePath}");
    }

    public static bool IsSceneExists(string scenePath)
    {
        // Scene 파일이 존재하는지 확인
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        if (sceneAsset == null)
        {
            return false;
        }
        return true;
    }
}
#endif