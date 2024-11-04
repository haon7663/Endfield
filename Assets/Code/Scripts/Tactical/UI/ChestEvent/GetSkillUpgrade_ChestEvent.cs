using System;
using UnityEngine;

public class GetSkillUpgrade_ChestEvent : ChestEvent
{
    public override Sprite Excute()
    {
        iconName = "SkillUpgrade";
        Debug.Log("��ų ���׷��̵� ȹ��");
        return base.Excute();
    }
    

}
