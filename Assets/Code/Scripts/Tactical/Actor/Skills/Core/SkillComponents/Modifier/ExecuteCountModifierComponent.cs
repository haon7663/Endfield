using System;
using UnityEngine;

public class ExecuteCountModifierComponent : ModifierComponent
{
    public override void ApplyModify(Skill targetSkill)
    {
        base.ApplyModify(targetSkill);
        switch (ExecuteType)
        {
            case SkillExecuteType.AddModifier:
                targetSkill.executeCount += value;
                break;
            case SkillExecuteType.MultiplyModifier:
                targetSkill.executeCount *= value;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}