using UnityEngine;

public abstract class ProjectileAttackComponent : AttackComponent
{
    public string prefabName;
    public int projectileSpeed;
    
    public override void Execute(SkillComponentInfo info)
    {
        base.Execute(info);
        
        var projectile = SkillFactory.Create(prefabName).GetComponent<Projectile>();
        projectile.Init(info, value, distance, projectileSpeed);
        
        executeObjects.Add(projectile.GetComponent<ISkillExecuter>());
        projectile.OnHit += HitParticle;
    } 
}