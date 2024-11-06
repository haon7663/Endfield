using System;

public class ElixirModifierComponent : ModifierComponent
{
    public override void ApplyModify(Skill targetSkill)
    {
        base.ApplyModify(targetSkill);
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
    }
}