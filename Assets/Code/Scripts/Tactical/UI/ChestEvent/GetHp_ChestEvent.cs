using System;
using UnityEngine;

public class GetHp_ChestEvent : ChestEvent
{
    public override (Sprite,string) Execute()
    {
        iconName = "30 체력 회복";
        GameManager.Inst.Player.Health.OnRecovery(30);
        return base.Execute();
    }
}
