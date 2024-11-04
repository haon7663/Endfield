using System;
using DG.Tweening;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action onHpChanged;
    public Action onDeath;
    
    private Unit _unit;
    private SpriteRenderer _spriteRenderer;
    
    public int curHp;
    public int maxHp;

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _spriteRenderer = _unit.SpriteTransform.GetComponent<SpriteRenderer>();
        curHp = maxHp;
    }

    public void OnDamage(int value)
    {
        curHp -= value;
        curHp = Mathf.Clamp(curHp, 0, maxHp);
        
        TextHudController.Inst.ShowDamage(transform.position + Vector3.up * 0.5f, value);
        CameraShake.Inst.Shake();

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
}
