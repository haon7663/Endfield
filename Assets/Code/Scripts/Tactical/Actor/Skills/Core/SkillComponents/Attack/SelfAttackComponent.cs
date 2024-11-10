using System.Collections.Generic;
using UnityEngine;

public class SelfAttackComponent : AttackComponent
{
    public override void Execute(SkillComponentInfo info)
    {
        if (info.user.TryGetComponent(out Health health))
        {
            health.OnDamage(value);
        }
    }

    public override void Cancel(SkillComponentInfo info)
    {
        GridManager.Inst.RevertGrid(info.user);
    }

    public override void Print(SkillComponentInfo info)
    {
        GridManager.Inst.ApplyGrid(info.user, new List<Tile> { info.user.Tile });
    }
}
