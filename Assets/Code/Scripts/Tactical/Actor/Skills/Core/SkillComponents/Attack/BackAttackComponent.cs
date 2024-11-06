using System.Collections.Generic;
using UnityEngine;

public class BackAttackComponent : AttackComponent
{
    public override void Execute(SkillComponentInfo info)
    {
        base.Execute(info);
        Debug.Log("BackAttack");
        
        for (var i = 1; i <= distance; i++)
        {
            var currentTile = GetStartingTile(info, -i);
            var targetUnit = currentTile?.content;
            if (targetUnit && targetUnit.TryGetComponent(out Health health))
            {
                var newInfo = new SkillComponentInfo(info, currentTile);
                OnHit?.Invoke(newInfo);
                
                health.OnDamage(value);
            }
        }
    }

    public override void Print(SkillComponentInfo info)
    {
        base.Print(info);
        
        var tiles = new List<Tile>();
        for (var i = 1; i <= distance; i++)
        {
            var tile = GetStartingTile(info, -i);
            if (tile)
                tiles.Add(tile);
        }
        GridManager.Inst.ApplyGrid(info.user, tiles);
    }
    
    public override void Cancel(SkillComponentInfo info)
    {
        base.Cancel(info);
        
        GridManager.Inst.RevertGrid(info.user);
    }
}