using System.Linq;
using UnityEngine;

public class MoveComponent : SkillComponent
{
    public override void Execute(SkillComponentInfo info)
    {
        var movement = info.user.Movement;
        var task = new Task(movement.OnMove(CalculateDistance(info) * info.dirX));
        task.Start();
    }

    public override void Print(SkillComponentInfo info)
    {
        var additionalDistance = CalculateDistance(info) * info.dirX;
        GridManager.Inst.DisplayPreview(info.user, info.tile.Key + additionalDistance);
    }

    public override void Cancel(SkillComponentInfo info)
    {
        base.Cancel(info);
        GridManager.Inst.RevertPreview(info.user);
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