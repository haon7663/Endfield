using System.Collections.Generic;
using UnityEngine;

public class LinearProjectileAttackComponent : ProjectileAttackComponent
{
    public override void Print(SkillComponentInfo info)
    {
        var tiles = new List<Tile>();
        for (var i = 1; i <= CalculateDistance(info); i++)
        {
            var tile = GetStartingTile(info, i);
            tiles.Add(tile);
        }
        GridManager.Inst.ApplyGrid(info.user, tiles);
    }
    
    public override void Cancel(SkillComponentInfo info)
    {
        GridManager.Inst.RevertGrid(info.user);
    }
    
    private int CalculateDistance(SkillComponentInfo info)
    {
        var index = 0;
        for (var i = 1; i <= distance; i++)
        {
            index = i;
            if (GetStartingTile(info, i).IsOccupied)
                break;
        }
        return index;
    }
}
