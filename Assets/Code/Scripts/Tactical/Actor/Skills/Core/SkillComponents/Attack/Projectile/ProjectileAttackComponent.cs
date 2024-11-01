using UnityEngine;

public class ProjectileAttackComponent : AttackComponent
{
    public string prefabName;
    public int projectileSpeed;
    
    public override void Execute(Unit user)
    {
        var projectile = SkillFactory.Create(prefabName).GetComponent<Projectile>();
        projectile.Init(user.Tile, user.GetComponent<Movement>().Dir, value, distance, projectileSpeed);
        ExecuteObjects.Add(projectile.GetComponent<IExecuteAble>());
    } 
}