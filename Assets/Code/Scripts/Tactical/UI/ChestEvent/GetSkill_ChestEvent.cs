using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GetSkill_ChestEvent : ChestEvent
{
    public override (Sprite,string) Execute()
    {
        var skills = SkillLoader.GetAllSkills("skill");
        var skill = skills[Random.Range(0, skills.Count)];
        showIconName = $"[{skill.label}] 카드 획득";
        SkillChangeController.Inst.Show(skill);
        return (SkillLoader.GetSkillSprite(skill.name),showIconName);
    }
}
