using System;
using UnityEngine;

public class GetGold_ChestEvent : ChestEvent
{
    public int givingGold = 300;
    

    public override (Sprite,string) Excute()
    {
        iconName = "Gold";
        GoldController.Inst.ReCountGold(+givingGold);
        return base.Excute();
    }

}
