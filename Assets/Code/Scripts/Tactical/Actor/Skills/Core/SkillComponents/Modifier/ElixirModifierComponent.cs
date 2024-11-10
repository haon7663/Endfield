using System;
using UnityEngine;

public class ElixirModifierComponent : ModifierComponent
{
    public override void ApplyModify(Skill targetSkill)
    {
        base.ApplyModify(targetSkill);
        Debug.Log($"Before: {targetSkill.elixir}");
        switch (ExecuteType)
        {
            case SkillExecuteType.AddModifier:
                targetSkill.elixir += value;
                break;
            case SkillExecuteType.MultiplyModifier:
                targetSkill.elixir *= value;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Debug.Log($"After: {targetSkill.elixir}");
    }
}