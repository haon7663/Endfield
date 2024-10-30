using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public static class SkillLoader
{
    public static List<Skill> GetSkillsFromUnitName(string unitName)
    {
        var skills = GetAllSkills("skill");
        return skills.Where(s => s.name.StartsWith("unitName")).ToList();
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
        var skillComponentsJson = skillCSV.ConvertCSVToJson();
        var skillComponentsData = JsonConvert.DeserializeObject<List<SkillComponent>>(skillComponentsJson, settings);
        
        var skills = new List<Skill>();
        foreach (var skillComponent in skillComponentsData)
        {
            var skill = skills.FirstOrDefault(s => s.name == skillComponent.saveName);

            if (skill != null)
            {
                skill.SkillComponents.Add(skillComponent);
            }
            else
            {
                var newSkill = skillData.FirstOrDefault(s => s.name == skillComponent.saveName);
                if (newSkill == null) continue;
                newSkill.SkillComponents.Add(skillComponent);
                skills.Add(newSkill);
            }
        }

        return skills;
    }
}