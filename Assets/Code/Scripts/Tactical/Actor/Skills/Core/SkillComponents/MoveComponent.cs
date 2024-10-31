using System.Linq;
using UnityEngine;

public class MoveComponent : SkillComponent
{
    private PreviewSprite _previewSprite;
    
    public override void Execute(Unit user)
    {
        user.additionalKey = 0;
        var movement = user.Movement;
        var task = new Task(movement.OnMove(CalculateDistance(user) * user.Movement.DirX));
        task.Start();
    }

    public override void Print(Unit user)
    {
        var additionalDistance = CalculateDistance(user) * user.Movement.DirX;
        _previewSprite = GridManager.Inst.DisplayPreview(user, user.Tile.Key + additionalDistance);
        user.additionalKey += additionalDistance;
    }

    public override void Cancel(Unit user)
    {
        _previewSprite?.Cancel();
        _previewSprite = null;
    }

    private int CalculateDistance(Unit user)
    {
        var index = 0;
        for (var i = 1; i <= distance; i++)
        {
            if (GetStartingTile(user, i).IsOccupied)
                break;
            index = i;
        }
        return index;
    }
}