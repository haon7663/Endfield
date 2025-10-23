#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class SettingControllerGeneratorEditor : EditorWindow
{
    private string scriptName = "NewScript";
    private string controllerPath = "Assets/_Project/Scripts/Controllers";


    [MenuItem("Tools/SettingController Generator")]
    public static void ShowWindow()
    {
        GetWindow<SettingControllerGeneratorEditor>("SettingController Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("SettingController 생성기", EditorStyles.boldLabel);

        scriptName = EditorGUILayout.TextField("SettingController 이름", scriptName);

        EditorGUILayout.Space();
        GUILayout.Label("경로 설정", EditorStyles.boldLabel);
        controllerPath = EditorGUILayout.TextField("Controller 경로", controllerPath);

        EditorGUILayout.Space();
        if (GUILayout.Button("SettingController 스크립트 생성"))
        {
            CreateSettingControllerScript();
        }
    }

    private void CreateSettingControllerScript()
    {
        if (!Directory.Exists(controllerPath))
        {
            Directory.CreateDirectory(controllerPath);
        }

        string scriptPath = Path.Combine(controllerPath, $"{scriptName}_SettingController.cs");
        string scriptContent = $@"using SandySetting;

public class {scriptName}_SettingController : SettingController
{{
    protected override void OnInitializeGame()
    {{
        base.OnInitializeGame();
        // 여기에 초기화 코드를 작성하세요
    }}
}}";

        File.WriteAllText(scriptPath, scriptContent);
        AssetDatabase.Refresh();
    }
}
#endif