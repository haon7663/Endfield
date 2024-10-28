using System.Linq;
using UnityEngine;

public class MoveComponent : SkillComponent
{
    public override void Execute(Unit user)
    {
        var movement = user.Movement;
        var task = new Task(movement.OnMove(CalculateDistance(user) * movement.DirX));
        task.Start();
    }

    public override void Print(Unit user)
    {
        
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