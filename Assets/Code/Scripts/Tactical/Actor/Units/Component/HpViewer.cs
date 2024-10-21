using UnityEngine;
using DG.Tweening;
using TMPro;

public class HpViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthTextPrefab;
    
    private Health health;
    private int _prevHp;

    private void Start()
    {
        health = GetComponent<Health>();
        HealthTextController.Inst.Connect(health);
        HealthTextController.Inst.UpdateUI(health, health.maxHp);
        health.damaged += UpdateHealthUI;
        _prevHp = health.maxHp;
    }

    private void UpdateHealthUI()
    {
        DOVirtual.Int(_prevHp, health.curHp, 0.3f, value => HealthTextController.Inst.UpdateUI(health, value))
            .SetEase(Ease.InOutQuad);
        _prevHp = health.curHp;
    }
}