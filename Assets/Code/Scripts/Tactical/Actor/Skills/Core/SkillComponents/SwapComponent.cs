using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Tactical.Actor.Tiles;
using UnityEngine;

public class SwapComponent : SkillComponent
{
    private PreviewSprite _previewSprite;
    
    public override void Execute(SkillComponentInfo info)
    {
        var isSwap = false;
        
        for (var i = 1; i <= distance; i++)
        {
            var targetTile = GetStartingTile(info, i);
            if (!targetTile || !targetTile.content) continue;
            
            var task = new Task(info.user.Movement.OnSwap(targetTile.content));
            task.Start();
            isSwap = true;
            break;
        }

        if (isSwap) return;
        {
            var task = new Task(info.user.Movement.OnMove(info.user.Movement.DirX));
            task.Start();
        }
    }

    public override void Print(SkillComponentInfo info)
    {
        var tiles = new List<Tile>();
        for (var i = 1; i <= distance; i++)
        {
            var tile = GetStartingTile(info, i);
            tiles.Add(tile);
            if (!tile || !tile.content) continue;
            break;
        }
        GridManager.Inst.ApplyGrid(info.user, tiles);
    }

    public override void Cancel(SkillComponentInfo info)
    {
        base.Cancel(info);
        GridManager.Inst.RevertGrid(info.user);
    }
}