using System.Collections.Generic;
using UnityEngine;

public class ThrowerProjectileAttackComponent : ProjectileAttackComponent
{
    public override void Print(Unit user)
    {
        GridManager.Inst.ApplyGrid(user, new List<Tile> { GetStartingTile(user, distance) });
    }
    public override void Cancel(Unit user)
    {
        GridManager.Inst.RevertGrid(user);
    }
}
