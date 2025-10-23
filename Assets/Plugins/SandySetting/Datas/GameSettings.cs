using System;
using SandyToolkitCore.Settings.Interface;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace SandySetting.Datas
{
    [Serializable]
    public partial class GameSettings : IGameSettings
    {
        public string Version { get; set; } = "0.1.0";
        public IAudioSetting Audio { get; set; }

        public string LanguageCode { get; set; }

        public GameSettings()
        {
            Audio = new AudioSettings();
            LanguageCode = LocalizationSettings.SelectedLocale.Identifier.Code;
        }
    }
}