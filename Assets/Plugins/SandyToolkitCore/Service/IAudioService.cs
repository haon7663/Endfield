using SandyToolkitCore.Models;
using SandyToolkitCore.Settings.Interface;
using SandyToolkitCore.Settings.Service;
using UnityEngine;

namespace SandyToolkitCore.Service
{
    public partial interface IAudioService
    {
        void Initialize(IAudioSettingService audioSettingService);
        void SetVolume(BusType busType, float volume);
        void SetAmbienceParameter(string parameterName, float value);
        void SetBGMParameter(string parameterName, float value);
        void SetParameterWithLabel(string parameterName, string label);
        void PlayOneShot(string soundKey, Vector3 worldPos);
        void PlayOneShot(string soundKey);
        void PlayBGM(string bgmKey);
        void PlayAmbient(string ambienceKey);
        void PlayOneShotCustom(string soundKey, float? pitch = null);
    }
}