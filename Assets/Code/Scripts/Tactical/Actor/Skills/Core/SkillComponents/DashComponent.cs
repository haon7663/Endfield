using UnityEngine;

public class DashComponent : SkillComponent
{
    public int damage;
    
    public override void Execute(Unit user)
    {
        if (user.TryGetComponent(out Movement movement))
        {
            movement.OnMove(distance);
        }
    }
}