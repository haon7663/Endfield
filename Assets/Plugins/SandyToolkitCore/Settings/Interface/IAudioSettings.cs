namespace SandyToolkitCore.Settings.Interface
{
    public interface IAudioSetting
    {
        float MasterVolume { get; set; }
        float BgmVolume { get; set; }
        float SfxVolume { get; set; }
        float AmbienceVolume { get; set; }
    }
} 