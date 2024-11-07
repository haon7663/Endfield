using System;
using UnityEngine;

public class GetSkillUpgrade_ChestEvent : ChestEvent
{
    public override (Sprite,string) Execute()
    {
        iconName = "스킬 업그레이드";
        DataManager.Inst.Data.skillUpgradeTickets++;
        return base.Execute();
    }
}
