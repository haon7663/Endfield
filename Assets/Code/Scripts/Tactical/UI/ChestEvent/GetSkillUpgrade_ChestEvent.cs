using System;
using UnityEngine;

public class GetSkillUpgrade_ChestEvent : ChestEvent
{
    public override (Sprite,string) Excute()
    {
        iconName = "SkillUpgrade";
        Debug.Log("��ų ���׷��̵� ȹ��");
        return base.Excute();
    }
    

}
