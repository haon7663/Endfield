using UnityEngine;

public class ProjectileAttackComponent : AttackComponent
{
    public string prefabName;
    public int projectileSpeed;
    
    public override void Execute(SkillComponentInfo info)
    {
        var projectile = SkillFactory.Create(prefabName).GetComponent<Projectile>();
        projectile.Init(info, value, distance, projectileSpeed);
        ExecuteObjects.Add(projectile.GetComponent<ISkillExecuter>());
    } 
}