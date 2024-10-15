using System;
using UnityEngine;

public class LinearProjectile : Projectile
{
    public override void Init(Tile tile, int damage, int distance, int projectileSeped)
    {
        base.Init(tile, damage, distance, projectileSeped);
        transform.position = tile.transform.position + Vector3.up * 0.5f;
    }

    private float _timer;
    private int _currentLocalKey;

    private void Update()
    {
        _currentLocalKey = Mathf.FloorToInt(_timer * ProjectileSpeed) + 1;
        if (_currentLocalKey > Distance)
            Destroy(gameObject);

        var prevPos = (Tile.transform.position + Vector3.right * (_currentLocalKey - 1)) + Vector3.up * 1.2f;
        var curPos = (Tile.transform.position + Vector3.right * _currentLocalKey) + Vector3.up * 1.2f;
        
        transform.position = Vector3.Lerp(prevPos, curPos, _timer * ProjectileSpeed - Mathf.FloorToInt(_timer * ProjectileSpeed));
        _timer += Time.deltaTime;
    }
}
