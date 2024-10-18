using System;
using UnityEngine;

public class ThrowerProjectile : Projectile
{
    public override void Init(Tile tile, Vector3 dir, int damage, int distance, int projectileSpeed)
    {
        base.Init(tile, dir, damage, distance, projectileSpeed);
        transform.position = tile.transform.position + Vector3.up * 1.2f;
        _startPosition = tile.transform.position + Vector3.up * 1.2f;
        _targetPosition = tile.transform.position + dir * distance + Vector3.up * 1.2f;
    }

    private float _timer;
    private int _currentLocalKey;

    private Vector3 _startPosition, _targetPosition;

    private void Update()
    {
        _currentLocalKey = Mathf.FloorToInt(_timer * projectileSpeed) + 1;
        if (_currentLocalKey > distance)
        {
            var currentTile = GridManager.Inst.GetTile(tile.Key + distance);
            if (currentTile && currentTile.content)
            {
                if (currentTile.content.TryGetComponent(out Health health))
                {
                    print(damage);
                    health.OnDamage(damage);
                }
            }
            Destroy(gameObject);
        }
        
        transform.position = Vector3.Slerp(_startPosition, _targetPosition, _timer / ((distance - 1f) / projectileSpeed));
        _timer += Time.deltaTime;
    }
}