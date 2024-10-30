using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    public Tile Tile { get; private set; }
    [HideInInspector] public int additionalKey;

    [field:SerializeField]
    public Transform SpriteTransform { get; private set; }
    public SpriteRenderer Renderer { get; private set; }
    public Movement Movement { get; private set; }
    public Health Health { get; private set; }
    public SkillHolder SkillHolder { get; private set; }

    public UnitType unitType;

    private void Awake()
    {
        Renderer = SpriteTransform.GetComponent<SpriteRenderer>();
        Movement = GetComponent<Movement>();
        Health = GetComponent<Health>();
        SkillHolder = GetComponent<SkillHolder>();
    }

    public void Init(UnitData data, Tile tile)
    {
        name = data.name;
        Debug.Log(data.animatorController);
        SpriteTransform.GetComponent<Animator>().runtimeAnimatorController = data.animatorController;
        Health.maxHp = Health.curHp = data.health;
        SkillHolder.skills = data.skills;

        if (TryGetComponent(out AIController aiController))
            aiController.actionCool = data.actionTime;
        
        Place(tile);
        transform.position = Tile.transform.position + Vector3.up * 0.5f;
    }

    public void Place(Tile tile)
    {
        if (Tile)
            Tile.content = null;
        
        Tile = tile;
        Tile.content = this;
    }
}
