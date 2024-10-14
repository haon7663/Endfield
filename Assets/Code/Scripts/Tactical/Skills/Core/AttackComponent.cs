using UnityEngine;

public enum AttackType
{
    Projectile,
    Appoint,
    Immediate
}

public class AttackComponent : SkillComponent
{
    //public AttackType attackType;
    //public string prefabName;
    public int damage;
    public int distance;
    
    public override void Execute(Unit user)
    {
        
    }
}