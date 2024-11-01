using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Projectile : MonoBehaviour, IExecuteAble
{
    protected Tile tile;
    protected Vector3 dir;
    protected int damage;
    protected int distance;
    protected int projectileSpeed;

    public virtual void Init(Tile tile, Vector3 dir, int damage, int distance, int projectileSpeed)
    {
        this.tile = tile;
        this.dir = dir;
        this.damage = damage;
        this.distance = distance;
        this.projectileSpeed = projectileSpeed;
    }

    public Action OnHit { get; set; }
    public Action OnEnd { get; set; }
}
