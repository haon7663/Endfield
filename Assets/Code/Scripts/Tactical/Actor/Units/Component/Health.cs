using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class BarrierDuration
{
    public int barrier;
    public float duration;

    public BarrierDuration(int barrier, float duration)
    {
        this.barrier = barrier;
        this.duration = duration;
    }
}

public class Health : MonoBehaviour
{
    public Action onHpChanged;
    public Action onDeath;
    
    private Unit _unit;
    private SpriteRenderer _spriteRenderer;
    
    public int curHp;
    public int maxHp;
    public List<BarrierDuration> barrierDurations = new List<BarrierDuration>();
    
    public float takeDamageMultiplier = 1;

    private void Start()
    {
        onDeath += () => SoundManager.Inst.Play("Dead");
        onDeath += () => GridManager.Inst.RevertGrid(_unit);
        onDeath += () => GridManager.Inst.RevertPreview(_unit);
              
        _unit = GetComponent<Unit>();
        _spriteRenderer = _unit.SpriteTransform.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (barrierDurations.Count > 0)
        {
            for (var i = barrierDurations.Count - 1; i >= 0; i--)
            {
                barrierDurations[i].duration -= Time.deltaTime;
                if (barrierDurations[i].duration <= 0)
                {
                    barrierDurations.RemoveAt(i);
                    onHpChanged?.Invoke();
                }
            }
        }
    }
    
    private IEnumerator HitMove(float duration, Vector3 direction, float strength, int vibrato, float randomness)
    {
        var interval = duration / vibrato / 3;
        for (var i = 0; i < vibrato; i++)
        {
            var randDirection = Quaternion.AngleAxis(Random.Range(-randomness, randomness), Vector3.forward) * direction;
            _unit.SpriteTransform.localPosition = randDirection * strength;
            yield return new WaitForSeconds(interval);
            _unit.SpriteTransform.localPosition = randDirection * -strength;
            yield return new WaitForSeconds(interval);
            _unit.SpriteTransform.localPosition = Vector3.zero;
            yield return new WaitForSeconds(interval);
        }
    }

    public void OnDamage(int value)
    {
        SoundManager.Inst.Play("Hitted");
        value = Mathf.RoundToInt(value * takeDamageMultiplier);

        for (var i = barrierDurations.Count - 1; i >= 0; i--)
        {
            var barrierDuration = barrierDurations[i];
            var saveDamage = value;
            
            value -= barrierDuration.barrier;
            barrierDuration.barrier -= saveDamage;
            
            if (barrierDuration.barrier <= 0)
                barrierDurations.RemoveAt(i);

            if (value > 0) continue;
            
            value = 0;
            break;
        }

        if (value > 0)
        {
            curHp -= value;
            curHp = Mathf.Clamp(curHp, 0, maxHp);

            var isPlayer = _unit.unitType == UnitType.Player;
            if (isPlayer) ArtDirectionManager.Inst.OnHit();
            
            StartCoroutine(HitMove(0.15f, Vector3.right, 0.25f, 2, 60));
            CameraShake.Inst.Shake(curHp <= 0 ? 0.4f : 0.2f, isPlayer);
            TextHudController.Inst.ShowDamage(transform.position + Vector3.up * 0.5f, value);
        }

        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
            {
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
            var isPlayer = _unit.unitType == UnitType.Player;
            if (!isPlayer) ArtDirectionManager.Inst.KillUnitTime();
            
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

    public void OnBarrier(int value, float duration)
    {
        barrierDurations.Add(new BarrierDuration(value, duration));
        
        TextHudController.Inst.ShowBarrier(transform.position + Vector3.up * 0.5f, value);
        
        onHpChanged?.Invoke();
    }
}
