using System;
using System.Collections.Generic;

namespace SandyAddressable.Data
{
    [Serializable]
    public class AddressableGroupData
    {
        public string groupName;
        public List<string> assetAddresses;  // 그룹에 포함된 에셋들의 주소 목록
    }
}