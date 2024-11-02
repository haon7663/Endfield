using System.Collections.Generic;
using UnityEngine;

public class RecoveryComponent : SkillComponent
{
    public int value;
    
    public override void Execute(SkillComponentInfo info)
    {
        if (distance == 0)
        {
            if (info.user.TryGetComponent(out Health health))
            {
                health.OnRecovery(value);
            }
        }
        else
        {
            for (var i = 1; i <= distance; i++)
            {
                var targetUnit = GetStartingTile(info, i)?.content;
                if (targetUnit && targetUnit.TryGetComponent(out Health health))
                {
                    health.OnRecovery(value);
                }
            }
        }
    }

    public override void Cancel(SkillComponentInfo info)
    {
        GridManager.Inst.RevertGrid(info.user);
    }

    public override void Print(SkillComponentInfo info)
    {
        var tiles = new List<Tile>();
        if (distance == 0)
        {
            tiles.Add(info.user.Tile);
        }
        else
        {
            for (var i = 1; i <= distance; i++)
            {
                var tile = GetStartingTile(info, i);
                if (tile)
                    tiles.Add(tile);
            }
        }
        
        GridManager.Inst.ApplyGrid(info.user, tiles);
    }
}