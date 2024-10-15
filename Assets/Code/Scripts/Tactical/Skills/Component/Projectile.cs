using UnityEngine;
using UnityEngine.Serialization;

public abstract class Projectile : MonoBehaviour
{
    protected Tile Tile;
    protected int Damage;
    protected int Distance;
    protected int ProjectileSpeed;

    public virtual void Init(Tile tile, int damage, int distance, int projectileSpeed)
    {
        Tile = tile;
        Damage = damage;
        Distance = distance;
        ProjectileSpeed = projectileSpeed;
    }
}
