using System.Collections.Generic;
using UnityEngine;

public class LinearProjectileAttackComponent : ProjectileAttackComponent
{
    public override void Print(Unit user)
    {
        var tiles = new List<Tile>();
        for (var i = 1; i <= CalculateDistance(user); i++)
        {
            var tile = GetStartingTile(user, i);
            tiles.Add(tile);
        }
        GridManager.Inst.DisplayGrid(user, tiles);
    }
    
    public override void Cancel(Unit user)
    {
        GridManager.Inst.RevertGrid(user);
    }
    
    private int CalculateDistance(Unit user)
    {
        var index = 0;
        for (var i = 1; i <= distance; i++)
        {
            index = i;
            if (GetStartingTile(user, i).IsOccupied)
                break;
        }
        return index;
    }
}
