using System;
using UnityEngine;

public class LinearProjectile : Projectile
{
    public override void Init(SkillComponentInfo info, int damage, int distance, int projectileSpeed)
    {
        base.Init(info, damage, distance, projectileSpeed);
        transform.position = tile.transform.position + Vector3.right * dirX + Vector3.up * 1.2f;
    }

    private float _timer;
    private int _currentLocalKey;

    private void Update()
    {
        _currentLocalKey = Mathf.FloorToInt(_timer * projectileSpeed) + 1;
        var currentTile = GridManager.Inst.GetTile(tile.Key + _currentLocalKey * dirX);
        if (_currentLocalKey >= distance)
        {
            OnEnd?.Invoke(new SkillComponentInfo(info, currentTile));
            Destroy(gameObject);
        }

        var prevPos = tile.transform.position + Vector3.right * (dirX * _currentLocalKey) + Vector3.up * 1.2f;
        var curPos = tile.transform.position + Vector3.right * (dirX * (_currentLocalKey + 1))  + Vector3.up * 1.2f;
        
        transform.position = Vector3.Lerp(prevPos, curPos, _timer * projectileSpeed - Mathf.FloorToInt(_timer * projectileSpeed));
        _timer += Time.deltaTime;
        
        if (currentTile && currentTile.content)
        {
            if (currentTile.content.TryGetComponent(out Health health))
            {
                health.OnDamage(damage);
                var newInfo = new SkillComponentInfo(info, currentTile);
                OnHit?.Invoke(newInfo);
                OnEnd?.Invoke(newInfo);
                Destroy(gameObject);
            }
        }
    }
}
