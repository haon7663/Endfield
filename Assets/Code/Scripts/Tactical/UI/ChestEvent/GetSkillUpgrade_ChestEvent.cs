using System;
using UnityEngine;

public class GetSkillUpgrade_ChestEvent : ChestEvent
{
    public override (Sprite,string) Execute()
    {
        iconName = "SkillUpgrade";
        showIconName = "스킬 업그레이드 획득";
        DataManager.Inst.Data.skillUpgradeTickets++;
        return base.Execute();
    }
}
