using System;
using UnityEngine;

public class GetSkillUpgrade_ChestEvent : ChestEvent
{
    public override (Sprite,string) Execute()
    {
        iconName = "SkillUpgrade";
        Debug.Log("��ų ���׷��̵� ȹ��");
        return base.Execute();
    }
}
