using System;
using UnityEngine;

public class GetGold_ChestEvent : ChestEvent
{
    public int givingGold;
    

    public override (Sprite,string) Excute()
    {
        iconName = "Gold";
        GoldController.Inst.ReCountGold(+givingGold);
        return base.Excute();
    }

}
