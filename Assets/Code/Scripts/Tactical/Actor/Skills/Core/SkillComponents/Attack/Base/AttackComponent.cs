using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : SkillComponent
{
    public int value;
    
    public override void Execute(SkillComponentInfo info)
    {
        for (var i = 1; i <= distance; i++)
        {
            var targetUnit = GetStartingTile(info, i)?.content;
            if (targetUnit && targetUnit.TryGetComponent(out Health health))
            {
                health.OnDamage(value);
            }
        }
    }

    public override void Print(SkillComponentInfo info)
    {
        var tiles = new List<Tile>();
        for (var i = 1; i <= distance; i++)
        {
            var tile = GetStartingTile(info, i);
            if (tile)
                tiles.Add(tile);
        }
        GridManager.Inst.ApplyGrid(info.user, tiles);
    }
    
    public override void Cancel(SkillComponentInfo info)
    {
        GridManager.Inst.RevertGrid(info.user);
    }
}