using System;
using DG.Tweening;
using UnityEngine;

public class Health : Singleton<Health>
{
    public Action onHpChanged;
    public Action onDeath;
    
    private Unit _unit;
    private SpriteRenderer _spriteRenderer;
    
    public int curHp;
    public int maxHp;
    
    public float takeDamageMultiplier = 1;

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _spriteRenderer = _unit.SpriteTransform.GetComponent<SpriteRenderer>();
        InitHp();
    }

    public void OnDamage(int value)
    {
        value = Mathf.RoundToInt(value * takeDamageMultiplier);
        
        curHp -= value;
        curHp = Mathf.Clamp(curHp, 0, maxHp);

        if (_unit.unitType == UnitType.Player)
        {
            ArtDirectionManager.Inst.OnHit();
            CameraShake.Inst.Shake(true);
        }
        else
        {
            CameraShake.Inst.Shake();
        }
        TextHudController.Inst.ShowDamage(transform.position + Vector3.up * 0.5f, value);

        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
            {
                _spriteRenderer.color = new Color(1, 0.25f, 0.25f);
                _spriteRenderer.material = Sprite2DMaterial.GetWhiteMaterial();
            })
            .AppendInterval(0.1f)
            .OnComplete(() =>
            {
                _spriteRenderer.color = Color.white;
                _spriteRenderer.material = Sprite2DMaterial.GetDefaultMaterial();
            });
        
        onHpChanged?.Invoke();

        if (curHp <= 0)
        {
            ParticleLoader.Create("Destroy", transform.position + Vector3.up, Quaternion.identity);
            onDeath?.Invoke();
            onDeath = null;
            Destroy(gameObject);
        }
    }
    public void OnRecovery(int value)
    {
        curHp += value;
        curHp = Mathf.Clamp(curHp, 0, maxHp);
        
        TextHudController.Inst.ShowRecovery(transform.position + Vector3.up * 0.5f, value);
        
        onHpChanged?.Invoke();
    }

    public void InitHp()
    {
        if(_unit.unitType == UnitType.Enemy)
        {
            curHp = maxHp;
        }
        else if(_unit.unitType == UnitType.Player)
        {
            curHp = DataManager.Inst.Data.curHp;
        }
    }
}
