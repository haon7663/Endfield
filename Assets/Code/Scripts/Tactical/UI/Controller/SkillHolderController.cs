using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class SkillHolderController : MonoBehaviour
{
    [SerializeField] private HealthText healthTextPrefab;
    [SerializeField] private Transform canvas;
    
    private Dictionary<Health, HealthText> _healthTexts = new Dictionary<Health, HealthText>();

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        foreach (var healthText in _healthTexts)
        {
            healthText.Value.transform.position = _mainCamera.WorldToScreenPoint(healthText.Key.transform.position);
        }
    }

    public void Connect(Health targetHealth)
    {
        var healthText = Instantiate(healthTextPrefab, canvas);
        _healthTexts.Add(targetHealth, healthText);
    }

    public void UpdateUI(Health targetHealth)
    {
        if (_healthTexts.TryGetValue(targetHealth, out var healthText))
        {
            healthText.UpdateUI(targetHealth.curHp, targetHealth.barrierDurations.Sum(bd => bd.barrier));
        }
    }

    public void DestroyUI(Health targetHealth)
    {
        _healthTexts[targetHealth].DOKill();
        Destroy(_healthTexts[targetHealth].gameObject);
        _healthTexts.Remove(targetHealth);
    }
}
