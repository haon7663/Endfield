using UnityEngine;

public class GetGold_ChestEvent : ChestEvent
{
    public int givingGold;

    public override void Excute()
    {
        Debug.Log("�� ����");
        GoldController.Inst.ReCountGold(+givingGold);
    }

}
