using System;
using UnityEngine;

public abstract class AttackComponent : SkillComponent, ISkillExecuter
{
    public int value;
    
    public override void Execute(SkillComponentInfo info)
    {
        AddOnHit(HitParticle);
    }

    public override void Print(SkillComponentInfo info) { }

    public override void Cancel(SkillComponentInfo info)
    {
        base.Cancel(info);
        
        OnHit = null;
        OnEnd = null;
    }

    public Action<SkillComponentInfo> OnHit { get; set; }
    public Action<SkillComponentInfo> OnEnd { get; set; }
    
    protected void HitParticle(SkillComponentInfo info)
    {
        var pos = info.tile.transform.position + Vector3.up * 0.75f;
        var rot = Quaternion.Euler(0, 0, info.tile.transform.position.x - info.user.transform.position.x > 0 ? 0 : 180);
        ParticleLoader.Create("Hit", pos, rot);
    }
}