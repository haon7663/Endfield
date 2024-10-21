using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class HealthTextController : Singleton<HealthTextController>
{
    [SerializeField] private TMP_Text healthTextPrefab;
    [SerializeField] private Transform canvas;
    
    private Dictionary<Health, TMP_Text> _healthTexts = new Dictionary<Health, TMP_Text>();

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

    public void UpdateUI(Health targetHealth, int hp)
    {
        _healthTexts[targetHealth].text = hp.ToString();
    }
}