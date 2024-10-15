using System;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Tile Tile { get; private set; }

    private void Start()
    {
        Place(GridManager.Inst.GetRandomTile());
        transform.position = Tile.transform.position + Vector3.up * 0.5f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var skill = SkillLoader.GetSkills("skill").FirstOrDefault(skill => skill.Name == "Slash");
            skill?.Use(this);
        }
    }

    public void Place(Tile tile)
    {
        if (Tile)
            Tile.content = null;
        
        Tile = tile;
        Tile.content = this;
    }
}
