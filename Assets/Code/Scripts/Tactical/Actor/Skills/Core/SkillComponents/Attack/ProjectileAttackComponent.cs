using UnityEngine;

public class ProjectileAttackComponent : AttackComponent
{
    public string prefabName;
    public int projectileSpeed;
    
    public override void Execute(Unit user)
    {
        var prefab = Resources.Load<GameObject>("Skills/" + prefabName);
        var projectile = SkillFactory.Create(prefabName).GetComponent<Projectile>();
        projectile.Init(user.Tile, user.GetComponent<Movement>().Dir, damage, distance, projectileSpeed);
    } 
}