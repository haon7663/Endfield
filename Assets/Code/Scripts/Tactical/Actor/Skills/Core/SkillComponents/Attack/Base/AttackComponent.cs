using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AttackComponent : SkillComponent, ISkillExecuter
{
    public int value;
    
    public override void Execute(SkillComponentInfo info)
    {
        AddOnHit(HitParticle);
        ExecuteObjects.Add(this);
    }

    public override void Print(SkillComponentInfo info) { }

    public Action<SkillComponentInfo> OnHit { get; set; }
    public Action<SkillComponentInfo> OnEnd { get; set; }
    
    protected void HitParticle(SkillComponentInfo info)
    {
        var pos = info.tile.transform.position + Vector3.up * 0.75f;
        var rot = Quaternion.Euler(0, 0, info.tile.transform.position.x - info.user.transform.position.x > 0 ? 0 : 180);
        ParticleLoader.Create("Hit", pos, rot);
    }
}