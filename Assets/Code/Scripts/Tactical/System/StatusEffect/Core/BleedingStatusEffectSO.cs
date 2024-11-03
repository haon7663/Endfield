using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/StatusEffect", fileName = "StatusEffect")]
public class BleedingStatusEffectSO : StatusEffectSO
{
    public int tickDamage;

    protected override void UpdateEffect(GameObject target)
    {
        base.UpdateEffect(target);

        if (target.TryGetComponent(out Health health))
        {
            if (isActive)
            {
                health.OnDamage(tickDamage);
            }
        }
    }
}