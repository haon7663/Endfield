#if UNITY_EDITOR
using SandyAddressable.Core;
using UnityEngine;
using UnityEditor;
using SandyAddressable.Data;

namespace SandyAddressable.Editor
{
    [CustomEditor(typeof(AddressableSystemConfig))]
    public class AddressableGroupManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var config = (AddressableSystemConfig)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Group Data", EditorStyles.boldLabel);
            var groupDataProperty = serializedObject.FindProperty("groupData");
            EditorGUILayout.PropertyField(groupDataProperty, true);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Scene Configs", EditorStyles.boldLabel);
            var sceneConfigsProperty = serializedObject.FindProperty("sceneConfigs");
            EditorGUILayout.PropertyField(sceneConfigsProperty, true);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

            if (GUILayout.Button("Initialize Group Data"))
            {
                var settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
                if (settings == null)
                {
                    Debug.LogError("Addressable Setting이 생성되지 않았습니다. Windows -> Asset Management -> Addresssables -> Groups을 클릭한 후 생성해주세요");
                    return;
                }

                config.GroupData.Clear();

                foreach (var group in settings.groups)
                {
                    if (group.ReadOnly) continue; // Default Local Group 등 제외

                    var groupInfo = new AddressableGroupData
                    {
                        groupName = group.Name,
                        assetAddresses = new System.Collections.Generic.List<string>()
                    };

                    foreach (var entry in group.entries)
                    {
                        groupInfo.assetAddresses.Add(entry.address);
                    }

                    config.GroupData.Add(groupInfo);
                }

                EditorUtility.SetDirty(config);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif