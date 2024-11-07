using System.Collections.Generic;
using UnityEngine;

public class RushComponent : SkillComponent
{
    public int value;
    private PreviewSprite _previewSprite;
    
    public override void Execute(SkillComponentInfo info)
    {
        var movement = info.user.Movement;
        
        var index = 0;
        for (var i = 1; i <= distance; i++)
        {
            var tile = GetStartingTile(info, i);
            if (tile && tile.IsOccupied)
            {
                tile.content.Health.OnDamage(value);
                break;
            }
            index = i;
        }
        
        var task = new Task(movement.OnMove(index * info.dirX));
        task.Start();
    }

    public override void Print(SkillComponentInfo info)
    {
        var additionalDistance = CalculateDistance(info) * info.dirX;
        _previewSprite = GridManager.Inst.DisplayPreview(info.user, info.tile.Key + additionalDistance);
        
        for (var i = 1; i <= distance; i++)
        {
            var tile = GetStartingTile(info, i);
            if (tile && tile.IsOccupied)
            {
                GridManager.Inst.ApplyGrid(info.user, new List<Tile> { tile });
                break;
            }
        }
    }

    public override void Cancel(SkillComponentInfo info)
    {
        base.Cancel(info);
        _previewSprite?.Cancel();
        _previewSprite = null;
        GridManager.Inst.RevertGrid(info.user);
    }

    private int CalculateDistance(SkillComponentInfo info)
    {
        var index = 0;
        for (var i = 1; i <= distance; i++)
        {
            var tile = GetStartingTile(info, i);
            if (tile && tile.IsOccupied)
                break;
            index = i;
        }
        return index;
    }
}