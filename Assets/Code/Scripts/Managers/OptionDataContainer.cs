using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class OptionDataContainer : MonoBehaviour
{
    public SoundSettingsData soundSettingsData;
    private static string _soundSettingsFilePath;

    private void Start()
    {
        _soundSettingsFilePath = Path.Combine(Application.persistentDataPath, "settings-sound.json");
        Load();
    }

    private void Load()
    {
        if (File.Exists(_soundSettingsFilePath))
        {
            var text = File.ReadAllText(_soundSettingsFilePath);
            soundSettingsData = JsonConvert.DeserializeObject<SoundSettingsData>(text);
        }
    }

    public void Save()
    {
        var json = JsonConvert.SerializeObject(soundSettingsData, Formatting.Indented);
        File.WriteAllText(_soundSettingsFilePath, json);
    }
}