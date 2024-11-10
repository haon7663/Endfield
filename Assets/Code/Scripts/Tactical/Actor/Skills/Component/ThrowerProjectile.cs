using System;
using DG.Tweening;
using UnityEngine;

public class ThrowerProjectile : Projectile
{
    public override void Init(SkillComponentInfo info, int damage, int distance, int projectileSpeed)
    {
        base.Init(info, damage, distance, projectileSpeed);
        transform.position = tile.transform.position + Vector3.up * 1.2f;
        _startPosition = tile.transform.position + Vector3.up * 1.2f;
        _targetPosition = tile.transform.position + Vector3.right * (dirX * distance) + Vector3.up * 1.2f;
    }

    private float _timer;
    private int _currentLocalKey;

    private Vector3 _startPosition, _targetPosition;

    private void Update()
    {
        _currentLocalKey = Mathf.FloorToInt(_timer * projectileSpeed) + 1;
        if (_currentLocalKey > distance)
        {
            var existTile = GridManager.Inst.TryGetTile(tile.Key + dirX * distance, out var currentTile);
            var newInfo = new SkillComponentInfo(info, currentTile);
            if (existTile)
            {
                if (currentTile.content && currentTile.content.TryGetComponent(out Health health))
                {
                    health.OnDamage(damage);
                    if (info.user.unitType == UnitType.Player)
                        SoundManager.Inst.Play("Enemy_Hit");
                    OnHit?.Invoke(newInfo);
                }
            }
            OnEnd?.Invoke(newInfo);
            Destroy(gameObject);
        }

        var startX = _startPosition.x;
        var targetX = _targetPosition.x;
        var nextX = Mathf.MoveTowards(transform.position.x, targetX, projectileSpeed * Time.deltaTime);
        var baseY = Mathf.Lerp(_startPosition.y, _targetPosition.y, (nextX - startX) / distance);
        var arc = 2 * (nextX - startX) * (nextX - targetX) / (-0.25f * distance * distance);
        var nextPosition = new Vector3(nextX, baseY + arc, transform.position.z);
        transform.rotation = LookAt2D(nextPosition - transform.position);
        transform.position = nextPosition;
        
        _timer += Time.deltaTime;
    }
    
    Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }
}