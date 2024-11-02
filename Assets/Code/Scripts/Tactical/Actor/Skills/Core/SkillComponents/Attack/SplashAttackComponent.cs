using System.Collections.Generic;

public class SplashAttackComponent : AttackComponent
{
    public override void Execute(SkillComponentInfo info)
    {
        for (var i = -distance; i <= distance; i++)
        {
            var targetUnit = GetStartingTile(info, i)?.content;
            if (targetUnit && targetUnit.TryGetComponent(out Health health))
            {
                health.OnDamage(value);
            }
        }
    }

    public override void Print(SkillComponentInfo info)
    {
        var tiles = new List<Tile>();
        for (var i = -distance; i <= distance; i++)
        {
            var tile = GetStartingTile(info, i);
            if (tile)
                tiles.Add(tile);
        }
        GridManager.Inst.ApplyGrid(info.user, tiles);
    }
}