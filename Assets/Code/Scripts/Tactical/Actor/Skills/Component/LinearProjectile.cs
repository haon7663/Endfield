using System;
using UnityEngine;

public class LinearProjectile : Projectile
{
    public bool isPenetrate;
    
    public override void Init(SkillComponentInfo info, int damage, int distance, int projectileSpeed)
    {
        base.Init(info, damage, distance, projectileSpeed);
        transform.position = tile.transform.position + Vector3.right * dirX + Vector3.up * 1.2f;
        transform.localScale = new Vector3(dirX, 1, 1);
    }

    private float _timer;
    private int _currentLocalKey;
    private bool _getDamaged;

    private void Update()
    {
        var localKey = Mathf.FloorToInt(_timer * projectileSpeed) + 1;
        if (_currentLocalKey != localKey)
        {
            _currentLocalKey = localKey;
            _getDamaged = false;
        }

        var prevPos = tile.transform.position + Vector3.right * (dirX * _currentLocalKey) + Vector3.up * 1.2f;
        var curPos = tile.transform.position + Vector3.right * (dirX * (_currentLocalKey + 1))  + Vector3.up * 1.2f;
        
        transform.position = Vector3.Lerp(prevPos, curPos, _timer * projectileSpeed - Mathf.FloorToInt(_timer * projectileSpeed));
        _timer += Time.deltaTime;
        
        if (_currentLocalKey > distance)
        {
            OnEnd?.Invoke(new SkillComponentInfo(info, GridManager.Inst.GetTile(tile.Key + _currentLocalKey * dirX)));
            Destroy(gameObject);
            return;
        }
        
        if (GridManager.Inst.TryGetTile(tile.Key + _currentLocalKey * dirX, out var currentTile))
        {
            if (!currentTile.content || !currentTile.content.TryGetComponent(out Health health)) return;
            if (_getDamaged) return;
            
            health.OnDamage(damage);
            if (info.user.unitType == UnitType.Player)
                SoundManager.Inst.Play("Enemy_Hit");
            var newInfo = new SkillComponentInfo(info, currentTile);
            OnHit?.Invoke(newInfo);
            _getDamaged = true;

            if (isPenetrate) return;
            OnEnd?.Invoke(newInfo);
            Destroy(gameObject);
        }
    }
}
