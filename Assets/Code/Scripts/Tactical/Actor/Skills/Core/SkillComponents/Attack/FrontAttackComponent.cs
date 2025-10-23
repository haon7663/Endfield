using System.Collections.Generic;
using Code.Scripts.Tactical.Actor.Tiles;

public class FrontAttackComponent : AttackComponent
{
    public override void Execute(SkillComponentInfo info)
    {
        base.Execute(info);
        
        for (var i = 1; i <= distance; i++)
        {
            var currentTile = GetStartingTile(info, i);
            var targetUnit = currentTile?.content;
            if (targetUnit && targetUnit.TryGetComponent(out Health health))
            {
                var newInfo = new SkillComponentInfo(info, currentTile);
                OnHit?.Invoke(newInfo);
                
                if (info.user.unitType == UnitType.Player)
                    SoundManager.Inst.Play("Enemy_Hit");
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
            var tile = GetStartingTile(info, i);
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