using UnityEngine;

public class DashComponent : SkillComponent
{
    public int damage;
    public int distance;
    
    public override void Execute(Unit user)
    {
        if (user.TryGetComponent(out Movement movement))
        {
            movement.OnMove(distance);
        }
    }

    public override void Print(Unit user)
    {
        throw new System.NotImplementedException();
    }
}