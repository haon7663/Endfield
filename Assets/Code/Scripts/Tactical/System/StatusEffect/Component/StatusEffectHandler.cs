using System.Collections.Generic;
using System.Linq;
using GDX.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatusEffectHandler : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<StatusEffectType, StatusEffectSO> statusEffectToApplyDict = new();
    private SerializableDictionary<StatusEffectType, StatusEffectSO> _enabledEffects = new();
    private Dictionary<StatusEffectType, StatusEffectSO> _statusEffectCacheDict = new();

    [SerializeField] private float interval = .1f;
    private float _currentInterval;

    public UnityAction<StatusEffectSO> ActivateStatus;
    public UnityAction<StatusEffectSO, float> UpdateStatus;
    public UnityAction<StatusEffectSO> DeactivateStatus;
    

    public void OnStatusTrigger(StatusEffectType effectType, float duration)
    {
        if (!_enabledEffects.ContainsKey(effectType))
        {
            var effectToAdd = CreateEffectObject(effectType, statusEffectToApplyDict[effectType]);
            _enabledEffects[effectType] = effectToAdd;
            
            ActivateStatus?.Invoke(effectToAdd);
        }

        if (!_enabledEffects[effectType].isActive)
        {
            _enabledEffects[effectType].ApplyEffect(gameObject, duration);
            UpdateStatus?.Invoke(_enabledEffects[effectType], _enabledEffects[effectType].GetCurrentDurationNormalized);
        }
        else
        {
            if (_enabledEffects[effectType].GetCurrentDuration < duration)
            {
                _enabledEffects[effectType].ApplyEffect(gameObject, duration);
                UpdateStatus?.Invoke(_enabledEffects[effectType], _enabledEffects[effectType].GetCurrentDurationNormalized);
            }
        }
        
        _currentInterval = 0;
    }

    private StatusEffectSO CreateEffectObject(StatusEffectType statusEffectType, StatusEffectSO statusEffect)
    {
        if (!_statusEffectCacheDict.ContainsKey(statusEffectType))
        {
            _statusEffectCacheDict[statusEffectType] = Instantiate(statusEffect);
        }
        return _statusEffectCacheDict[statusEffectType];
    }

    public void UpdateEffects(GameObject target)
    {
        foreach (var effect in _enabledEffects.ToList())
        {
            effect.Value.UpdateCall(target, interval);
            
            UpdateStatus?.Invoke(effect.Value, effect.Value.GetCurrentDurationNormalized);
            
            if (!effect.Value.isActive)
                RemoveEffect(effect.Key);
        }
    }
    
    public void RemoveEffect(StatusEffectType effectType)
    {
        if (_enabledEffects.ContainsKey(effectType))
        {
            _enabledEffects[effectType].RemoveEffect(gameObject);
            
            DeactivateStatus?.Invoke(_enabledEffects[effectType]);

            _enabledEffects.Remove(effectType);
        }
    }

    private void Update()
    {
        if (_enabledEffects.Count <= 0) return;
        
        _currentInterval += Time.deltaTime;
        if (_currentInterval > interval)
        {
            UpdateEffects(gameObject);
            _currentInterval = 0;
        }
    }
}