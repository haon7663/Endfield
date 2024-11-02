using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackComponent : SkillComponent, ISkillExecuter
{
    public int value;
    
    public override void Execute(SkillComponentInfo info)
    {
        for (var i = 1; i <= distance; i++)
        {
            var targetTile = GetStartingTile(info, i);
            var targetUnit = targetTile?.content;
            if (targetUnit && targetUnit.TryGetComponent(out Health health))
            {
                health.OnDamage(value);
                OnHit?.Invoke(new SkillComponentInfo(info, targetTile));
            }
        }
    }

    public override void Print(SkillComponentInfo info)
    {
        var tiles = new List<Tile>();
        for (var i = 1; i <= distance; i++)
        {
            tiles.Add(GetStartingTile(info, i));
        }
        GridManager.Inst.ApplyGrid(info.user, tiles);
    }
    
    public override void Cancel(SkillComponentInfo info)
    {
        GridManager.Inst.RevertGrid(info.user);
    }

    public Action<SkillComponentInfo> OnHit { get; set; }
    public Action<SkillComponentInfo> OnEnd { get; set; }
}