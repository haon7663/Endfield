using System.Collections.Generic;
using UnityEngine;

public class LinearProjectileAttackComponent : AttackComponent
{
    public string prefabName;
    public int projectileSpeed;
    public int isPenetrate;
    
    protected Projectile projectile;

    public override void Init(SkillComponentInfo info)
    {
        base.Init(info);
        
        projectile = SkillFactory.Create(prefabName).GetComponent<Projectile>();
        executeObjects.Add(projectile.GetComponent<ISkillExecuter>());
        projectile.OnHit += HitParticle;
    }
    
    public override void Execute(SkillComponentInfo info)
    {
        base.Execute(info);
        
        projectile?.Init(info, value, distance, projectileSpeed);
        if (projectile is LinearProjectile linearProjectile)
        {
            linearProjectile.isPenetrate = isPenetrate == 1;
        }
    } 
    
    public override void Print(SkillComponentInfo info)
    {
        base.Print(info);
        
        var tiles = new List<Tile>();
        for (var i = 1; i <= (isPenetrate == 1 ? distance : CalculateDistance(info)); i++)
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
    
    private int CalculateDistance(SkillComponentInfo info)
    {
        var index = 0;
        for (var i = 1; i <= distance; i++)
        {
            index = i;
            var tile = GetStartingTile(info, i);
            if (tile && tile.IsOccupied)
                break;
        }
        return index;
    }
}
