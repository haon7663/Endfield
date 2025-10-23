using System.Collections.Generic;
using Code.Scripts.Tactical.Actor.Tiles;
using UnityEngine;

public class ThrowerProjectileAttackComponent : ProjectileAttackComponent
{
    public override void Print(SkillComponentInfo info)
    {
        base.Print(info);
        
        var tile = GetStartingTile(info, distance);
        if (tile)
            GridManager.Inst.ApplyGrid(info.user, new List<Tile> { tile });
    }
    public override void Cancel(SkillComponentInfo info)
    {
        base.Cancel(info);
        
        GridManager.Inst.RevertGrid(info.user);
    }
}
