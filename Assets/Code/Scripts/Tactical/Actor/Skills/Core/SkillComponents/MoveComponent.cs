using System.Linq;
using UnityEngine;

public class MoveComponent : SkillComponent
{
    private PreviewSprite _previewSprite;
    
    public override void Execute(SkillComponentInfo info)
    {
        var movement = info.user.Movement;
        var task = new Task(movement.OnMove(CalculateDistance(info) * info.dirX));
        task.Start();
    }

    public override void Print(SkillComponentInfo info)
    {
        var additionalDistance = CalculateDistance(info) * info.dirX;
        _previewSprite = GridManager.Inst.DisplayPreview(info.user, info.tile.Key + additionalDistance);
    }

    public override void Cancel(SkillComponentInfo info)
    {
        _previewSprite?.Cancel();
        _previewSprite = null;
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