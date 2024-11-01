using System.Collections.Generic;
using UnityEngine;

public class ThrowerProjectileAttackComponent : ProjectileAttackComponent
{
    public override void Print(SkillComponentInfo info)
    {
        GridManager.Inst.ApplyGrid(info.user, new List<Tile> { GetStartingTile(info, distance) });
    }
    public override void Cancel(SkillComponentInfo info)
    {
        GridManager.Inst.RevertGrid(info.user);
    }
}
