
using UnityEngine;

namespace SandyToolkitCore.Settings.Interface
{
    public partial interface IGameSettings
    {
        string Version { get; }
        IAudioSetting Audio { get; }
        string LanguageCode { get; set; }
    }
}

