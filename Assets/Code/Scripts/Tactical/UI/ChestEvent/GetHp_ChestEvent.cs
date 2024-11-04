using System;
using UnityEngine;

public class GetHp_ChestEvent : ChestEvent
{
    public override Sprite Excute()
    {
        iconName = "Hp";
        GameManager.Inst.Player.Health.OnRecovery(1000);
        return base.Excute();
    }
}
