using UnityEngine;
using DG.Tweening;
using TMPro;

public class HpViewer : MonoBehaviour
{
    private Unit _unit;
    private Health _health;

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _health = GetComponent<Health>();
        
        HealthTextController.Inst.Connect(_health);
        if(_unit.unitType == UnitType.Enemy)
        {
            HealthTextController.Inst.UpdateUI(_health);
        }
        else if(_unit.unitType == UnitType.Player)
        {
            HealthTextController.Inst.UpdateUI(_health);
        }


        _health.onHpChanged += UpdateHealthUI;
        _health.onDeath += () =>
        {
            transform.DOKill(this);
            HealthTextController.Inst.Destroy(_health);
            GridManager.Inst.RevertGrid(_unit);
            SkillManager.Inst.RevertSkillArea(_unit);
        };
    }

    private void UpdateHealthUI()
    {
        //DOVirtual.Int(_prevHp, _health.curHp, 0.3f, value => HealthTextController.Inst.UpdateUI(_health, value)).SetEase(Ease.InOutQuad);
        HealthTextController.Inst.UpdateUI(_health);
    }
}