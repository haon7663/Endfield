using System;
using UnityEngine;

public class GetGold_ChestEvent : ChestEvent
{
    public int givingGold = 300;
    

    public override (Sprite,string) Execute()
    {
        showIconName = $"{givingGold} 골드 획득";
        iconName = "Gold";
        GoldController.Inst.ReCountGold(+givingGold);
        return base.Execute();
    }

}
