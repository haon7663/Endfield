using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : SkillComponent
{
    public int value;
    
    public override void Execute(Unit user)
    {
        for (var i = 1; i <= distance; i++)
        {
            var targetUnit = GridManager.Inst.GetTile(user.Tile.Key + i * user.Movement.DirX)?.content;
            if (targetUnit && targetUnit.TryGetComponent(out Health health))
            {
                health.OnDamage(value);
            }
        }
    }

    public override void Cancel(Unit user)
    {
        GridManager.Inst.RevertGrid(user);
    }

    public override void Print(Unit user)
    {
        var tiles = new List<Tile>();
        for (var i = 1; i <= distance; i++)
        {
            var tile = GridManager.Inst.GetTile(user.Tile.Key + i * user.Movement.DirX + user.additionalKey);
            tiles.Add(tile);
        }
        GridManager.Inst.ApplyGrid(user, tiles);
    }
}