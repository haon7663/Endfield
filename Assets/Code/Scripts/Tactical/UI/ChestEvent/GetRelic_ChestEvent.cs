using NUnit.Framework;
using UnityEngine;

public class GetRelic_ChestEvent : ChestEvent
{
    public override (Sprite, string) Execute()
    {
        //유물 이름, 스프이트 가져오기

        var relics =  ArtifactManager.Inst.GetAllRelics();
        var relic = relics[Random.Range(0, relics.Count)];
        relic.Execute();

        return (relic.sprite, relic.name);
    }
}
