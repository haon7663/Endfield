using SandyCore;
using SandyToolkitCore.Models;
using SandyToolkitCore.Pooling;
using SandyToolkitCore.Settings.Service;
using UnityEngine;

public static class AudioBridge
{
    public static void Initialize(IAudioSettingService audioSettingService)
    {
        ApplicationManager.Instance.AudioService.Initialize(audioSettingService);
    }

    public static void SetVolume(BusType busType, float volume)
    {
        ApplicationManager.Instance.AudioService.SetVolume(busType, volume);
    }

    public static void SetAmbienceParameter(string parameterName, float value)
    {
        ApplicationManager.Instance.AudioService.SetAmbienceParameter(parameterName, value);
    }

    public static void SetBGMParameter(string parameterName, float value)
    {
        ApplicationManager.Instance.AudioService.SetBGMParameter(parameterName, value);
    }

    public static void SetParameterWithLabel(string parameterName, string label)
    {
        ApplicationManager.Instance.AudioService.SetParameterWithLabel(parameterName, label);
    }

    public static void PlayOneShot(string soundKey, Vector3 worldPos)
    {
        ApplicationManager.Instance.AudioService.PlayOneShot(soundKey, worldPos);
    }

    public static void PlayOneShot(string soundKey)
    {
        ApplicationManager.Instance.AudioService.PlayOneShot(soundKey);
    }

    public static void PlayBGM(string bgmKey)
    {
        ApplicationManager.Instance.AudioService.PlayBGM(bgmKey);
    }

    public static void PlayAmbient(string ambienceKey)
    {
        ApplicationManager.Instance.AudioService.PlayAmbient(ambienceKey);
    }
}