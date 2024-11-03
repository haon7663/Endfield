using System;
using System.Collections.Generic;
using GDX.Collections.Generic;
using UnityEngine;

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] private StatusEffectIcon statusEffectIconPrefab;
    
    private Dictionary<StatusEffectSO, StatusEffectIcon> _statusEffectToIconDict;

    private StatusEffectHandler _statusEffectHandler;

    private void Start()
    {
        _statusEffectToIconDict = new Dictionary<StatusEffectSO, StatusEffectIcon>();
        _statusEffectHandler = GetComponentInParent<StatusEffectHandler>();

        _statusEffectHandler.ActivateStatus += OnActivateStatus;
        _statusEffectHandler.UpdateStatus += OnUpdateStatus;
        _statusEffectHandler.DeactivateStatus += OnDeActivateStatus;
    }

    private StatusEffectIcon GetStatusIcon(StatusEffectSO statusEffect)
    {
        if (_statusEffectToIconDict.TryGetValue(statusEffect, out var icon))
            return icon;
        
        var statusIcon = Instantiate(statusEffectIconPrefab, transform);
        statusIcon.SetSprite(statusEffect.sprite);
        return statusIcon;
    }

    private void OnActivateStatus(StatusEffectSO statusEffect)
    {
        var statusEffectIcon = GetStatusIcon(statusEffect);
        _statusEffectToIconDict[statusEffect] = statusEffectIcon;
        _statusEffectToIconDict[statusEffect].gameObject.SetActive(true);
    }

    private void OnUpdateStatus(StatusEffectSO statusEffect, float duration)
    {
        _statusEffectToIconDict[statusEffect].UpdateFill(duration);
    }
    
    private void OnDeActivateStatus(StatusEffectSO statusEffect)
    {
        _statusEffectToIconDict[statusEffect].gameObject.SetActive(false);
    }
}