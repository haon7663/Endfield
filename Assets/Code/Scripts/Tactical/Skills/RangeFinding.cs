using System.Collections.Generic;
using UnityEngine;

public static class RangeFinding
{
    public static List<Tile> GetAreaInRange(this Tile target, int range, bool onSelf = true)
    {
        var tiles = new List<Tile>();
        
        if (onSelf)
            tiles.Add(target);
        for (var i = 1; i <= range; i++)
        {
            tiles.Add(GridManager.Inst.GetTile(target.Key + i));
            tiles.Add(GridManager.Inst.GetTile(target.Key - i));
        }

        return tiles;
    }
    
    public static List<Tile> GetTilesOnRange(this Tile target, int range)
    {
        var tiles = new List<Tile>
        {
            GridManager.Inst.GetTile(target.Key + range),
            GridManager.Inst.GetTile(target.Key - range)
        };
        return tiles;
    }
}
