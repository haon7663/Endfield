using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public List<Skill> skills;

    public int eliteKillCount;
    public int plantKillCount;
    public int artifactCount = ArtifactManager.Inst.ArtifactCount();
    public int skillCount;
    public int stageCount;
    public int gold = 150;
    public int curHp;
    public int skillUpgradeTickets;
    public int maxElixir = 4;
    
    public PlayerData(List<Skill> skills)
    {
        this.skills = skills;
    }
}

public class DataManager : SingletonDontDestroyOnLoad<DataManager>
{
    public PlayerData Data { get; private set; }
    private string _playerDataFilePath;

    protected override void Awake()
    {
        base.Awake();

        _playerDataFilePath = Path.Combine(Application.persistentDataPath, "data.json");

        if (File.Exists(_playerDataFilePath))
        {
            var playerDataJson = File.ReadAllText(_playerDataFilePath);
            var playerData = JsonConvert.DeserializeObject<PlayerData>(playerDataJson);

            Data = playerData;
        }
        else
        {
            Generate("Player");
            //임시, 나중에 게임 처음 시작할 때 생성하면 된다.
        }
        Data.curHp = UnitLoader.GetUnitData("Player").health;
    }

    public void Generate(string unitName)
    {
        var startingSkills = UnitLoader.GetUnitData(unitName).skills;
        var playerData = new PlayerData(startingSkills);
        Data = playerData;
    }

    public void Save()
    {
        if (Data == null) return;

        var json = JsonConvert.SerializeObject(Data, Formatting.Indented,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

        File.WriteAllText(_playerDataFilePath, json);
    }

    public void ResetData()
    {
        Data.curHp = 60;
        Data.eliteKillCount = 0;
        Data.plantKillCount = 0;
        Data.artifactCount = 0;
        Data.skillCount = 0;
        Data.stageCount = 0;
        Data.gold = 150;
        Data.skillUpgradeTickets = 0;
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
