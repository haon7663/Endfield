using UnityEngine;

public class AttackComponent : SkillComponent
{
    public int damage;
    
    public override void Execute(Unit user)
    {
        var targetUnit = GridManager.Inst.GetTile(user.Tile.Key + distance).content;
        if (targetUnit == null) return;
        if (targetUnit.TryGetComponent(out Health health))
        {
            health.OnDamage(damage);
        }
    }

    public override void Print(Unit user)
    {
        throw new System.NotImplementedException();
    }
}