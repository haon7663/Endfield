using UnityEngine;

public abstract class ProjectileAttackComponent : AttackComponent
{
    public string prefabName;
    public int projectileSpeed;

    private Projectile _projectile;

    public override void Init(SkillComponentInfo info)
    {
        base.Init(info);
        
        _projectile = SkillFactory.Create(prefabName).GetComponent<Projectile>();
        executeObjects.Add(_projectile.GetComponent<ISkillExecuter>());
        _projectile.OnHit += HitParticle;
    }
    
    public override void Execute(SkillComponentInfo info)
    {
        base.Execute(info);
        
        _projectile?.Init(info, value, distance, projectileSpeed);
    } 
}