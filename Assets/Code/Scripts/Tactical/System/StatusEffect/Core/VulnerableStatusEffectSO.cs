using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/StatusEffect", fileName = "StatusEffect")]
public class VulnerableStatusEffectSO : StatusEffectSO
{
    protected override void UpdateEffect(GameObject target)
    {
        base.UpdateEffect(target);
        
        Debug.Log("VulnerableStatusEffectSO.UpdateEffect");

        if (isActive)
        {
            if (target.TryGetComponent(out Health health))
                health.takeDamageMultiplier = 1.5f;
        }
    }
    
    public override void RemoveEffect(GameObject target)
    {
        base.RemoveEffect(target);
        
        Debug.Log("VulnerableStatusEffectSO.RemoveEffect");

        if (target.TryGetComponent(out Health health))
            health.takeDamageMultiplier = 1f;
    }
}