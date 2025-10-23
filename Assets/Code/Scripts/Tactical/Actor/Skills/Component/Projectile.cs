using System;
using Code.Scripts.Tactical.Actor.Tiles;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Projectile : MonoBehaviour, ISkillExecuter
{
    protected SkillComponentInfo info;
    protected Tile tile;
    protected int dirX;
    protected int damage;
    protected int distance;
    protected int projectileSpeed;

    public virtual void Init(SkillComponentInfo info, int damage, int distance, int projectileSpeed)
    {
        this.info = info;
        tile = info.tile;
        dirX = info.dirX;
        this.damage = damage;
        this.distance = distance;
        this.projectileSpeed = projectileSpeed;
    }

    public Action<SkillComponentInfo> OnHit { get; set; }
    public Action<SkillComponentInfo> OnEnd { get; set; }
}
