using System;
using SandyToolkitCore.Settings.Interface;
using SandyToolkitCore.Settings.Service;

namespace SandySetting.Service
{

    public class AudioSettingService : IAudioSettingService
    {
        private IAudioSetting _setting;

        public AudioSettingService(IAudioSetting setting)
        {
            _setting = setting;
        }

        public float MasterVolume
        {
            get
            {
                return _setting.MasterVolume;
            }
            set
            {
                _setting.MasterVolume = value;
                OnMasterVolumeChanged?.Invoke();
            }
        }
        public float BgmVolume
        {
            get
            {
                return _setting.BgmVolume;
            }
            set
            {
                _setting.BgmVolume = value;
                OnBgmVolumeChanged?.Invoke();
            }
        }
        public float SfxVolume
        {
            get
            {
                return _setting.SfxVolume;
            }
            set
            {
                _setting.SfxVolume = value;
                OnSfxVolumeChanged?.Invoke();
            }
        }
        public float AmbienceVolume
        {
            get
            {
                return _setting.AmbienceVolume;
            }
            set
            {
                _setting.AmbienceVolume = value;
                OnAmbienceVolumeChanged?.Invoke();
            }
        }

        public event Action OnMasterVolumeChanged;
        public event Action OnBgmVolumeChanged;
        public event Action OnSfxVolumeChanged;
        public event Action OnAmbienceVolumeChanged;
    }
}