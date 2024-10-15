using UnityEngine;

public class AttackComponent : SkillComponent
{
    public int damage;
    public int distance;
    
    public override void Execute(Unit user)
    {
        var targetUnit = GridManager.Inst.GetTile(user.Tile.Key + distance).content;
        if (targetUnit == null) return;
        if (targetUnit.TryGetComponent(out Health health))
        {
            health.OnDamage(damage);
        }
    }
}