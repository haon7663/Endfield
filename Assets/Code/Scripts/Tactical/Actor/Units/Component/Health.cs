using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    public Action damaged;
    
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

    public void OnDamage(int damage)
    {
        curHp -= damage;
        
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
        
        damaged?.Invoke();
    }
}
