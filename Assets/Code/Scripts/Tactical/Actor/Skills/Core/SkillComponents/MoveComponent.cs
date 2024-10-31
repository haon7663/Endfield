using System.Linq;
using UnityEngine;

public class MoveComponent : SkillComponent
{
    private PreviewSprite _previewSprite;
    
    public override void Execute(Unit user)
    {
        var movement = user.Movement;
        var task = new Task(movement.OnMove(CalculateDistance(user) * movement.DirX));
        task.Start();
    }

    public override void Print(Unit user)
    {
        var additionalDistance = CalculateDistance(user) * user.Movement.DirX;
        _previewSprite = GridManager.Inst.DisplayPreview(user, user.Tile.Key + additionalDistance);
        user.additionalKey = additionalDistance;
    }

    public override void Cancel(Unit user)
    {
        _previewSprite?.Cancel();
        _previewSprite = null;
        user.additionalKey = 0;
    }

    private int CalculateDistance(Unit user)
    {
        var ableDistance = 0;
        for (var i = 1; i <= distance; i++)
        {
            if (GridManager.Inst.GetTile(user.Tile.Key + i * user.Movement.DirX).IsOccupied)
                break;
                
            ableDistance = i;
        }
        return ableDistance;
    }
}