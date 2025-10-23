using System;
using SandyCore;

public static class SettingBridge
{
    public static float MasterVolume
    {
        get
        {
            return ApplicationManager.Instance.SettingService.AudioSetting.MasterVolume;
        }
        set
        {
            ApplicationManager.Instance.SettingService.AudioSetting.MasterVolume = value;
        }
    }
    public static float BgmVolume
    {
        get
        {
            return ApplicationManager.Instance.SettingService.AudioSetting.BgmVolume;
        }
        set
        {
            ApplicationManager.Instance.SettingService.AudioSetting.BgmVolume = value;
        }
    }
    public static float SfxVolume
    {
        get
        {
            return ApplicationManager.Instance.SettingService.AudioSetting.SfxVolume;
        }
        set
        {
            ApplicationManager.Instance.SettingService.AudioSetting.SfxVolume = value;
        }
    }
    public static float AmbienceVolume
    {
        get
        {
            return ApplicationManager.Instance.SettingService.AudioSetting.AmbienceVolume;
        }
        set
        {
            ApplicationManager.Instance.SettingService.AudioSetting.AmbienceVolume = value;
        }
    }
    public static event Action OnMasterVolumeChanged
    {
        add
        {
            ApplicationManager.Instance.SettingService.AudioSetting.OnMasterVolumeChanged += value;
        }
        remove
        {
            ApplicationManager.Instance.SettingService.AudioSetting.OnMasterVolumeChanged -= value;
        }
    }
    public static event Action OnBgmVolumeChanged
    {
        add
        {
            ApplicationManager.Instance.SettingService.AudioSetting.OnBgmVolumeChanged += value;
        }
        remove
        {
            ApplicationManager.Instance.SettingService.AudioSetting.OnBgmVolumeChanged -= value;
        }
    }
    public static event Action OnSfxVolumeChanged
    {
        add
        {
            ApplicationManager.Instance.SettingService.AudioSetting.OnSfxVolumeChanged += value;
        }
        remove
        {
            ApplicationManager.Instance.SettingService.AudioSetting.OnSfxVolumeChanged -= value;
        }
    }
}
