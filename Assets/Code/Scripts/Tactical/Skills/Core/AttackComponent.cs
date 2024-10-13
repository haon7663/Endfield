using UnityEngine;

public class AttackComponent : SkillComponent
{
    public int damage;
    public int distance;
    
    public override void Execute(Unit user)
    {
        Debug.Log("{damage} 데미지!");
    }
}