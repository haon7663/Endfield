using System;
using UnityEngine;

public class GetGold_ChestEvent : ChestEvent
{
    public int givingGold;
    

    public override Sprite Excute()
    {
        iconName = "gold";
        GoldController.Inst.ReCountGold(+givingGold);
        return base.Excute();
    }

}
