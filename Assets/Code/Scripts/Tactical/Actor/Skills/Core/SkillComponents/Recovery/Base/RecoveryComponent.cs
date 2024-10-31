using System.Collections.Generic;
using UnityEngine;

public class RecoveryComponent : SkillComponent
{
    public int value;
    
    public override void Execute(Unit user)
    {
        if (distance == 0)
        {
            if (user.TryGetComponent(out Health health))
            {
                health.OnRecovery(value);
            }
        }
        else
        {
            for (var i = 1; i <= distance; i++)
            {
                var targetUnit = GetStartingTile(user, i)?.content;
                if (targetUnit && targetUnit.TryGetComponent(out Health health))
                {
                    health.OnRecovery(value);
                }
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
        if (distance == 0)
        {
            tiles.Add(user.Tile);
        }
        else
        {
            for (var i = 1; i <= distance; i++)
            {
                var tile = GetStartingTile(user, i);
                tiles.Add(tile);
            }
        }
        
        GridManager.Inst.ApplyGrid(user, tiles);
    }
}