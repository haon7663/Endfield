using UnityEngine;
using UnityEngine.Serialization;

public abstract class Projectile : MonoBehaviour
{
    protected Tile Tile;
    protected Vector3 Dir;
    protected int Damage;
    protected int Distance;
    protected int ProjectileSpeed;

    public virtual void Init(Tile tile, Vector3 dir, int damage, int distance, int projectileSpeed)
    {
        Tile = tile;
        Dir = dir;
        Damage = damage;
        Distance = distance;
        ProjectileSpeed = projectileSpeed;
    }
}
