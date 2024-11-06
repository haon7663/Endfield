using System;
using Unity.VisualScripting;
using UnityEngine;

public class DamageModifierComponent : ModifierComponent
{
    public override void UpdateModify(SkillComponent targetComponent)
    {
        base.UpdateModify(targetComponent);

        if (targetComponent is AttackComponent attackComponent)
        {
            Debug.Log($"Before: {attackComponent.value}");
            switch (ExecuteType)
            {
                case SkillExecuteType.AddModifier:
                    attackComponent.value += value;
                    attackComponent.addedValue += value;
                    break;
                case SkillExecuteType.MultiplyModifier:
                    var saveValue = attackComponent.value;
                    attackComponent.value += (saveValue - attackComponent.addedValue) * (value - 1);
                    attackComponent.addedValue += (saveValue - attackComponent.addedValue) * (value - 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Debug.Log($"After: {attackComponent.value}");
        }
    }
}