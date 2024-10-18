using System;
using UnityEngine;

public class LinearProjectile : Projectile
{
    public override void Init(Tile tile, Vector3 dir, int damage, int distance, int projectileSpeed)
    {
        base.Init(tile, dir, damage, distance, projectileSpeed);
        transform.position = tile.transform.position + dir + Vector3.up * 1.2f;
    }

    private float _timer;
    private int _currentLocalKey;

    private void Update()
    {
        _currentLocalKey = Mathf.FloorToInt(_timer * projectileSpeed) + 1;
        if (_currentLocalKey > distance)
            Destroy(gameObject);

        var prevPos = (base.tile.transform.position + dir * _currentLocalKey) + Vector3.up * 1.2f;
        var curPos = (base.tile.transform.position + dir * (_currentLocalKey + 1))  + Vector3.up * 1.2f;
        
        transform.position = Vector3.Lerp(prevPos, curPos, _timer * projectileSpeed - Mathf.FloorToInt(_timer * projectileSpeed));
        _timer += Time.deltaTime;

        var currentTile = GridManager.Inst.GetTile(tile.Key + _currentLocalKey * (int)dir.x);
        if (currentTile && currentTile.content)
        {
            if (currentTile.content.TryGetComponent(out Health health))
            {
                print(damage);
                health.OnDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
