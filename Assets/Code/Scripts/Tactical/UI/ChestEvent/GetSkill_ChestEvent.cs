using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GetSkill_ChestEvent : ChestEvent
{


    public override Sprite Excute()
    {
        var skills = SkillLoader.GetAllSkills("skill");
        var skill = skills[Random.Range(0, skills.Count)];
        iconName = skill.name;
        DataManager.Inst.Data.skills.Add(skill);
        return SkillLoader.GetSkillSprite(iconName);
    }
    
    
}
