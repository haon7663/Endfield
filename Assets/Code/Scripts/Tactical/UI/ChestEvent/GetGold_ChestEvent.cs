using System;
using UnityEngine;

public class GetGold_ChestEvent : ChestEvent
{
    public int givingGold = 300;
    

    public override (Sprite,string) Execute()
    {
        iconName = $"{givingGold} 골드 획득";
        GoldController.Inst.ReCountGold(+givingGold);
        return base.Execute();
    }

}
