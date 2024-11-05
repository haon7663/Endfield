using System;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    public Tile Tile { get; private set; }
    public int additionalKey;

    [field: SerializeField]
    public Transform SpriteTransform { get; private set; }
    public SpriteRenderer Renderer { get; private set; }
    public Movement Movement { get; private set; }
    public Health Health { get; private set; }

    public Action OnAction;

    public UnitType unitType;

    private void Awake()
    {
        Renderer = SpriteTransform.GetComponent<SpriteRenderer>();
        Movement = GetComponent<Movement>();
        Health = GetComponent<Health>();
        if(unitType == UnitType.Player)
        {
            HealthTextController.Inst.UpdateUI(Health);
        }
    }

    private void OnDestroy()
    {
        CountEnemyKill();
    }

    public void Init(UnitData data, Tile tile)
    {
        name = data.name;
        SpriteTransform.GetComponent<Animator>().runtimeAnimatorController = data.animatorController;
        Health.maxHp = data.health;

        if (unitType == UnitType.Enemy)
        {
            Health.curHp = Health.maxHp;
        }
        else if (unitType == UnitType.Player)
        {
            Health.curHp = DataManager.Inst.Data.curHp;
        } 
        if (TryGetComponent(out SkillHolder skillHolder))
            skillHolder.skills = data.skills;
        if (TryGetComponent(out AIController aiController))
            aiController.actionCool = data.actionTime;
        Movement.isAnchored = data.isAnchored;

        transform.position = tile.transform.position;
        Place(tile.IsOccupied ? GridManager.Inst.FindNearestTile(tile.Key) : tile);
        transform.DOMove(Tile.transform.position + Vector3.up * 0.5f, 0.25f);

        OnAction += SkillManager.Inst.UpdateSkillArea;     
    }

    public void Place(Tile tile)
    {
        if (Tile)
            Tile.content = null;

        Tile = tile;
        Tile.content = this;

        OnAction?.Invoke();
    }

    public void Swap(Unit other)
    {
        (other.Tile, Tile) = (Tile, other.Tile);
        (other.Tile.content, Tile.content) = (Tile.content, other.Tile.content);

        OnAction?.Invoke();
    }

    public void CountEnemyKill()
    {
        if (unitType == UnitType.Enemy)
        {
            DataManager.Inst.Data.plantKillCount++;
        }
    }
}
