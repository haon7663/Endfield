using System;
using DG.Tweening;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public int curHp;
    public int maxHp;

    private void Start()
    {
        curHp = maxHp;
    }

    public void OnDamage(int damage)
    {
        curHp -= damage;

        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
            {
                spriteRenderer.color = new Color(1, 0.25f, 0.25f);
                spriteRenderer.material = Sprite2DMaterial.GetWhiteMaterial();
            })
            .AppendInterval(0.1f)
            .OnComplete(() =>
            {
                spriteRenderer.color = Color.white;
                spriteRenderer.material = Sprite2DMaterial.GetDefaultMaterial();
            });
    }
}
