using UnityEngine;
using DG.Tweening;
using TMPro;

public class HpViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthTextPrefab;
    
    private Health _health;
    private int _prevHp;

    private void Start()
    {
        _health = GetComponent<Health>();
        HealthTextController.Inst.Connect(_health);
        HealthTextController.Inst.UpdateUI(_health, _health.maxHp);
        _health.damaged += UpdateHealthUI;
        _health.onDeath += () =>
        {
            transform.DOKill(this);
            HealthTextController.Inst.DestroyUI(_health);
        };
        _prevHp = _health.maxHp;
    }

    private void UpdateHealthUI()
    {
        //DOVirtual.Int(_prevHp, _health.curHp, 0.3f, value => HealthTextController.Inst.UpdateUI(_health, value)).SetEase(Ease.InOutQuad);
        HealthTextController.Inst.UpdateUI(_health, _health.curHp);
        _prevHp = _health.curHp;
    }
}