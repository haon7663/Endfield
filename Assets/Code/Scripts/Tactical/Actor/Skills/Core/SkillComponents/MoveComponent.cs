using UnityEngine;

public class MoveComponent : SkillComponent
{
    public int distance;
    
    public override void Execute(Unit user)
    {
        if (user.TryGetComponent(out Movement movement))
        {
            movement.OnMove(distance * movement.DirX);
        }
    }

    public override void Print(Unit user)
    {
        throw new System.NotImplementedException();
    }
}