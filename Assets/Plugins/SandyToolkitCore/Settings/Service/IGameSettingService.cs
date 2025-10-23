
using System;
using UnityEngine;

namespace SandyToolkitCore.Settings.Service
{
    public partial interface IGameSettingService
    {
        string Version { get; }
        string LanguageCode { get; set; }

        event Action<string> OnLanguageChanged;
    }
}
