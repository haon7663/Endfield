using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/StatusEffect", fileName = "StatusEffect")]
public class BleedingStatusEffectSO : StatusEffectSO
{
    public int tickDamage;
    public GameObject tickEffectPrefab;

    protected override void UpdateEffect(GameObject target)
    {
        base.UpdateEffect(target);

        if (target.TryGetComponent(out Health health))
        {
            if (isActive)
            {
                health.OnDamage(tickDamage);
                
                if (tickEffectPrefab)
                {
                    Instantiate(tickEffectPrefab, target.transform.position + new Vector3(0, 0.75f), Quaternion.identity);
                }
            }
        }
    }
}