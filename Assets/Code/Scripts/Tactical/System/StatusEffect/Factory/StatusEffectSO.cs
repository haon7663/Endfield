using UnityEngine;

public abstract class StatusEffectSO : ScriptableObject
{
    public StatusEffectType statueEffectType;

    public Sprite sprite;
    public GameObject visualEffectPrefab;

    private float _activeDuration;
    private float _remainingDuration;
    private GameObject _vfxPlaying;

    public float tickInterval = .5f;
    private float _tickInterval;

    [HideInInspector] public bool isActive;

    public virtual void ApplyEffect(GameObject target, float duration)
    {
        isActive = true;
        _activeDuration = duration;
        _remainingDuration = _activeDuration;

        if (visualEffectPrefab != null)
        {
            _vfxPlaying = Instantiate(visualEffectPrefab, target.transform.position, Quaternion.identity,
                target.transform);
        }
    }

    public void UpdateCall(GameObject target, float tickAmount)
    {
        _remainingDuration -= tickAmount;
        if (_remainingDuration <= 0)
        {
            isActive = false;
        }

        _tickInterval += tickAmount;
        if (_tickInterval >= tickInterval)
        {
            UpdateEffect(target);
            _tickInterval = 0;
        }
    }

    protected virtual void UpdateEffect(GameObject target) { }

    public virtual void RemoveEffect(GameObject target)
    {
        _remainingDuration = 0;

        if (_vfxPlaying)
        {
            Destroy(_vfxPlaying);
        }
    }

    public float GetCurrentDurationNormalized => _remainingDuration / _activeDuration;
    public float GetCurrentDuration => _remainingDuration;
}