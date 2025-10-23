using System.Collections.Generic;
using Code.Scripts.Tactical.Actor.Tiles;
using UnityEngine;

public class BarrierComponent : SkillComponent
{
    public int value;
    public int duration;
    
    public override void Execute(SkillComponentInfo info)
    {
        if (info.user.TryGetComponent(out Health health))
        {
            health.OnBarrier(value, duration);
            SoundManager.Inst.Play("Barrier");
        }
    }

    public override void Cancel(SkillComponentInfo info)
    {
        GridManager.Inst.RevertGrid(info.user);
    }

    public override void Print(SkillComponentInfo info)
    {
        GridManager.Inst.ApplyGrid(info.user, new List<Tile> { info.user.Tile });
    }
}