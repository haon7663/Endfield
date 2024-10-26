using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public List<Skill> skills = new List<Skill>();
}

public class DataManager : SingletonDontDestroyOnLoad<DataManager>
{
    public PlayerData Data { get; private set; }
    private string _playerDataFilePath;
    
    protected override void Awake()
    {
        base.Awake();
        
        //임시
        Data = new PlayerData();
        //임시
        
        _playerDataFilePath = Path.Combine(Application.persistentDataPath, "data.json");
        
        if (File.Exists(_playerDataFilePath))
        {
            var playerDataJson = File.ReadAllText(_playerDataFilePath);
            var playerData = JsonConvert.DeserializeObject<PlayerData>(playerDataJson);

            Data = playerData;
            Data.skills = new List<Skill>();
        }
    }
    
    public void Save()
    {
        if (Data == null) return;

        var json = JsonConvert.SerializeObject(Data, Formatting.Indented,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        
        File.WriteAllText(_playerDataFilePath, json);
    }

    public void Delete()
    {
        File.Delete(_playerDataFilePath);
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        //Save();
#endif
    }
}
