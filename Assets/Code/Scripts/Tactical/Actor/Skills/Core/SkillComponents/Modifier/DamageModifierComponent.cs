using System;

public class DamageModifierComponent : ModifierComponent
{
    public override void UpdateModify(SkillComponent targetComponent)
    {
        base.UpdateModify(targetComponent);

        if (targetComponent is AttackComponent attackComponent)
        {
            switch (ExecuteType)
            {
                case SkillExecuteType.AddModifier:
                    attackComponent.value += value;
                    break;
                case SkillExecuteType.MultiplyModifier:
                    attackComponent.value *= value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}