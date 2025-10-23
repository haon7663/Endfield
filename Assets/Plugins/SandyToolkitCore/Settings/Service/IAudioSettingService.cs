using System;

namespace SandyToolkitCore.Settings.Service
{
    public partial interface IAudioSettingService
    {
        float MasterVolume { get; set; }
        float BgmVolume { get; set; }
        float SfxVolume { get; set; }
        float AmbienceVolume { get; set; }

        event Action OnMasterVolumeChanged;
        event Action OnBgmVolumeChanged;
        event Action OnSfxVolumeChanged;
        event Action OnAmbienceVolumeChanged;
    }
}