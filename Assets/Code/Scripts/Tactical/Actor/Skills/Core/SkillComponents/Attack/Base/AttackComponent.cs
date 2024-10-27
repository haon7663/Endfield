using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : SkillComponent
{
    public int damage;
    
    public override void Execute(Unit user)
    {
        for (var i = 1; i <= distance; i++)
        {
            var targetUnit = GridManager.Inst.GetTile(user.Tile.Key + i * user.Movement.DirX)?.content;
            if (targetUnit && targetUnit.TryGetComponent(out Health health))
            {
                health.OnDamage(damage);
            }
        }
    }

    public override void Print(Unit user)
    {
        var tiles = new List<Tile>();
        for (var i = 1; i <= distance; i++)
        {
            var tile = GridManager.Inst.GetTile(user.Tile.Key + i * user.Movement.DirX);
            tiles.Add(tile);
        }
        GridManager.Inst.DisplayGrid(user, tiles);
    }
}