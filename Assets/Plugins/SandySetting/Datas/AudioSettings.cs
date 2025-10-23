using SandyToolkitCore.Settings.Interface;
using System;

namespace SandySetting.Datas
{
    [Serializable]
    public class AudioSettings : IAudioSetting
    {
        public float MasterVolume { get; set; } = 1.0f;
        public float BgmVolume { get; set; } = 1.0f;
        public float SfxVolume { get; set; } = 1.0f;
        public float AmbienceVolume { get; set; } = 1.0f;
    }
}