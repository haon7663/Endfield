using UnityEngine;

public abstract class ProjectileAttackComponent : AttackComponent
{
    public string prefabName;
    public int projectileSpeed;

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
    } 
}