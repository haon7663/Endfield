using Cysharp.Threading.Tasks;
using SandyToolkitCore.Settings.Interface;
using SandyToolkitCore.Settings.Service;

namespace SandyToolkitCore.Service
{
    public partial interface ISettingService
    {
        IAudioSettingService AudioSetting { get; }
        IGameSettingService GameSetting { get; }
    }
}