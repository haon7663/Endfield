using System;
using UnityEngine;

public class GetHp_ChestEvent : ChestEvent
{
    public override (Sprite,string) Execute()
    {
        showIconName = "30 체력 회복";
        iconName = "Hp";
        GameManager.Inst.Player.Health.OnRecovery(30);
        DataManager.Inst.Data.curHp += 30;
        DataManager.Inst.Data.curHp = Mathf.Clamp(DataManager.Inst.Data.curHp, 0, GameManager.Inst.Player.Health.maxHp);
        return base.Execute();
    }
}
