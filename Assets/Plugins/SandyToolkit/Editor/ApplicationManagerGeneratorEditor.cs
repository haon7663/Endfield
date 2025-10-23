#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class ApplicationManagerGeneratorEditor : EditorWindow
{
    private string scriptName = "NewScript";
    private string managerPath = "Assets/_Project/Scripts/Managers";


    [MenuItem("Tools/ApplicationManager Generator")]
    public static void ShowWindow()
    {
        GetWindow<ApplicationManagerGeneratorEditor>("ApplicationManager Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("ApplicationManager 생성기", EditorStyles.boldLabel);

        scriptName = EditorGUILayout.TextField("ApplicationManager 이름", scriptName);

        EditorGUILayout.Space();
        GUILayout.Label("경로 설정", EditorStyles.boldLabel);
        managerPath = EditorGUILayout.TextField("Manager 경로", managerPath);

        EditorGUILayout.Space();
        if (GUILayout.Button("ApplicationManager 스크립트 생성"))
        {
            CreateApplicationManagerScript();
        }
    }

    private void CreateApplicationManagerScript()
    {
        if (!Directory.Exists(managerPath))
        {
            Directory.CreateDirectory(managerPath);
        }

        string scriptPath = Path.Combine(managerPath, $"{scriptName}_ApplicationManager.cs");
        string scriptContent = $@"using SandyCore;
        using SandyToolkitCore;

public class {scriptName}_ApplicationManager : ApplicationManager
{{
    protected override void OnHandleBackProcessController(BaseController controller)
    {{
        base.OnHandleBackProcessController(controller);
    }}

    protected override void OnHandleSceneController(BaseController controller)
    {{
        base.OnHandleSceneController(controller);
    }}
}}";

        File.WriteAllText(scriptPath, scriptContent);
        AssetDatabase.Refresh();
    }
}
#endif