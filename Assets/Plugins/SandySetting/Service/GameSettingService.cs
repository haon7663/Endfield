
using System;
using SandyToolkitCore.Settings.Interface;
using SandyToolkitCore.Settings.Service;
using UnityEngine;

namespace SandySetting.Service
{
    public class GameSettingService : IGameSettingService
    {
        private IGameSettings _setting;

        public GameSettingService(IGameSettings setting)
        {
            _setting = setting;
        }

        public string Version => _setting.Version;

        public string LanguageCode
        {
            get => _setting.LanguageCode;
            set
            {
                _setting.LanguageCode = value;
                OnLanguageChanged?.Invoke(value);
            }
        }

        public event Action<string> OnLanguageChanged;
    }
}
