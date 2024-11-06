using UnityEngine;

public class GetRelic_ChestEvent : ChestEvent
{
    public override (Sprite, string) Excute()
    {

        //유물 이름, 스프이트 가져오기
        iconName = "";

        return (SkillLoader.GetSkillSprite(iconName), iconName);
    }
}
