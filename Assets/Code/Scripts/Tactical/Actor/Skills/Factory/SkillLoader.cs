using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public static class SkillLoader
{
    public static Sprite GetSkillSprite(string name)
    {
        var sprites = Resources.LoadAll<Sprite>("Card_Icons");
        return sprites.FirstOrDefault(sprite => sprite.name == name);
    }
    
    public static Skill GetSkill(string skillName, bool isEnemy = false)
    {
        var skills = GetAllSkills(isEnemy ? "enemySkill" : "skill");
        return skills.FirstOrDefault(s => s.name.StartsWith(skillName));
    }
    
    public static List<Skill> GetAllSkills(string path)
    {
        if (!path.StartsWith("SkillData/"))
            path = "SkillData/" + path;
        
        var skillCSV = Resources.Load<TextAsset>(path);
        return GetAllSkills(skillCSV);
    }
    
    private static List<Skill> GetAllSkills(TextAsset skillCSV)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            SerializationBinder = new SkillComponentSerializationBinder()
        };

        var skillJson = skillCSV.ConvertCSVToSkillJson();
        var skillData = JsonConvert.DeserializeObject<List<Skill>>(skillJson, settings);
        var skillComponentsJson = skillCSV.ConvertCSVSkillComponentToJson();
        var skillComponentsData = JsonConvert.DeserializeObject<List<SkillComponent>>(skillComponentsJson, settings);
        
        var skills = new List<Skill>();
        foreach (var skillComponent in skillComponentsData)
        {
            var skill = skills.FirstOrDefault(s => s.name == skillComponent.saveName);

            if (skill != null)
            {
                skill.skillComponents.Add(skillComponent);
            }
            else
            {
                var newSkill = skillData.FirstOrDefault(s => s.name == skillComponent.saveName);
                if (newSkill == null) continue;
                newSkill.skillComponents.Add(skillComponent);
                skills.Add(newSkill);
            }
        }

        return skills;
    }
    
    public static List<SkillComponent> GetAllSkillComponent(string path)
    {
        if (!path.StartsWith("SkillData/"))
            path = "SkillData/" + path;
        
        var skillCSV = Resources.Load<TextAsset>(path);
        return GetAllSkillComponent(skillCSV);
    }

    private static List<SkillComponent> GetAllSkillComponent(TextAsset skillCSV)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            SerializationBinder = new SkillComponentSerializationBinder()
        };
        
        var skillComponentsJson = skillCSV.ConvertCSVSkillComponentToJson();
        var skillComponentsData = JsonConvert.DeserializeObject<List<SkillComponent>>(skillComponentsJson, settings);

        return skillComponentsData;
    }
}