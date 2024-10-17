using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public static class SkillLoader
{
    public static List<Skill> GetSkills(string path)
    {
        if (!path.StartsWith("SkillData/"))
            path = "SkillData/" + path;
        
        var skillCSV = Resources.Load<TextAsset>(path);
        return GetSkills(skillCSV);
    }
    
    private static List<Skill> GetSkills(TextAsset skillCSV)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            SerializationBinder = new SkillComponentSerializationBinder()
        };
        
        var skillJson = skillCSV.ConvertCSVToJson();
        var data = JsonConvert.DeserializeObject<List<SkillComponent>>(skillJson, settings);

        var skills = new List<Skill>();
        foreach (var skillComponent in data)
        {
            var skill = skills.FirstOrDefault(s => s.name == skillComponent.saveName);

            if (skill != null)
            {
                skill.SkillComponents.Add(skillComponent);
            }
            else
            {
                var newSkill = new Skill(skillComponent.saveName);
                newSkill.SkillComponents.Add(skillComponent);
                skills.Add(newSkill);
            }
        }

        return skills;
    }
}