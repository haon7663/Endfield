using UnityEngine;

public class GetGold_ChestEvent : ChestEvent
{
    public int givingGold;

    public override void Excute()
    {
        Debug.Log("±Ý ¾òÀ½");
        GoldController.Inst.ReCountGold(+givingGold);
    }

}
