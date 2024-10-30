using System;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Tile Tile { get; private set; }
    
    [field:SerializeField]
    public Transform SpriteTransform { get; private set; }
    public Renderer Renderer { get; private set; }
    public Movement Movement { get; private set; }
    public Health Health { get; private set; }
    public SkillHolder SkillHolder { get; private set; }

    public UnitType unitType;

    private void Awake()
    {
        Renderer = SpriteTransform.GetComponent<Renderer>();
        Movement = GetComponent<Movement>();
        Health = GetComponent<Health>();
        SkillHolder = GetComponent<SkillHolder>();
    }

    public void Init(UnitData data, Tile tile)
    {
        name = data.Name;
        Debug.Log(data.AnimatorController);
        SpriteTransform.GetComponent<Animator>().runtimeAnimatorController = data.AnimatorController;
        Health.maxHp = Health.curHp = data.Health;
        SkillHolder.skills = data.Skills;
        
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
