using UnityEngine;

public class DashComponent : SkillComponent
{
    public int damage;
    public int distance;
    
    public override void Execute(Unit user)
    {
        if (user.TryGetComponent(out Movement movement))
        {
            movement.OnMove(GridManager.Inst.GetTile(user.Tile.Key + distance));
        }
    }
}