using System.Collections.Generic;
using UnityEngine;

public class BerriorComponent : SkillComponent
{
    public int value;
    
    public override void Execute(SkillComponentInfo info)
    {
        if (info.user.TryGetComponent(out Health health))
        {
            health.OnRecovery(value);
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