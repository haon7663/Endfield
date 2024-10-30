using System;
using DG.Tweening;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action damaged;
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

    public void OnDamage(int damage)
    {
        curHp -= damage;
        
        TextHudController.Inst.ShowDamage(transform.position + Vector3.up * 0.5f, damage);
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

        if (curHp <= 0)
        {
            onDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}
