using System.Collections.Generic;
using UnityEngine;

public class DashComponent : SkillComponent
{
    public int value;
    
    private PreviewSprite _previewSprite;
    
    public override void Execute(Unit user)
    {
        var movement = user.Movement;
        var task = new Task(movement.OnMove(CalculateDistance(user).Item2 * movement.DirX));
        task.Start();
    }

    public override void Print(Unit user)
    {
        var tileAndDist = CalculateDistance(user);
        var additionalDistance = tileAndDist.Item2 * user.Movement.DirX;
        _previewSprite = GridManager.Inst.DisplayPreview(user, user.Tile.Key + additionalDistance);
        user.additionalKey = additionalDistance;
        
        GridManager.Inst.DisplayGrid(user, tileAndDist.Item1);
    }

    public override void Cancel(Unit user)
    {
        _previewSprite.Cancel();
        _previewSprite = null;
        user.additionalKey = 0;
        
        GridManager.Inst.RevertGrid(user);
    }

    private (List<Tile>, int) CalculateDistance(Unit user)
    {
        var ableDistance = 0;
        var tiles = new List<Tile>();
        for (var i = 1; i <= distance; i++)
        {
            var tile = GridManager.Inst.GetTile(user.Tile.Key + i * user.Movement.DirX);
            tiles.Add(tile);
            if (tile.IsOccupied)
                break;
                
            ableDistance = i;
        }
        return (tiles, ableDistance);
    }
}