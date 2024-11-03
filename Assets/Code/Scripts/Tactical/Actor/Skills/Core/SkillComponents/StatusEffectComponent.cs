using System;
using UnityEngine;

public class StatusEffectComponent : SkillComponent
{
    public int value;
    public string statusEffectType;
    
    public override void Execute(SkillComponentInfo info)
    {
        Debug.Log("Status0");
        if (info.tile.content && info.tile.content.TryGetComponent(out StatusEffectHandler statusEffectHandler))
        {
            Debug.Log("Status1");
            if (Enum.TryParse(statusEffectType, out StatusEffectType type))
            {
                Debug.Log(info.tile.content.name + " / " + type);
                statusEffectHandler.OnStatusTrigger(type, value);
            }
        }
    }

    public override void Print(SkillComponentInfo info)
    {
        
    }
}