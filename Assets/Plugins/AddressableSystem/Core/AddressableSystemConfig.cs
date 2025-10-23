using UnityEngine;
using System.Collections.Generic;
using SandyAddressable.Data;

#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
#endif

namespace SandyAddressable.Core
{
    [CreateAssetMenu(fileName = "AddressableSystemConfig", menuName = "Sandy/Addressable System Config")]
    public class AddressableSystemConfig : ScriptableObject
    {
        public const string RESOURCES_PATH = "Addressable/AddressableSystemConfig";

        [SerializeField] private List<AddressableGroupData> groupData = new List<AddressableGroupData>();
        [SerializeField] private List<SceneGroupConfig> sceneConfigs = new List<SceneGroupConfig>();

        public List<AddressableGroupData> GroupData => groupData;
        public List<SceneGroupConfig> SceneConfigs => sceneConfigs;

#if UNITY_EDITOR
        public void InitializeGroupData()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("Addressable Setting이 생성되지 않았습니다. Windows -> Asset Management -> Addresssables -> Groups을 클릭한 후 생성해주세요");
                return;
            }

            groupData.Clear();

            foreach (var group in settings.groups)
            {
                if (group.ReadOnly) continue; // Default Local Group 등 제외

                var groupInfo = new AddressableGroupData
                {
                    groupName = group.Name,
                    assetAddresses = new List<string>()
                };

                foreach (var entry in group.entries)
                {
                    groupInfo.assetAddresses.Add(entry.address);
                }

                groupData.Add(groupInfo);
            }
        }
#endif
    }
}