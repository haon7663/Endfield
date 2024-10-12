using System;
using UnityEngine;

public enum Role {player,enemy }

public class Unit : MonoBehaviour
{
    public Role role;
    public Tile Tile { get; private set; }

    private void Start()
    {
        if(role== Role.player)
            Place(GridManager.Inst.GetTile(20));
        else Place(GridManager.Inst.GetTile(0));
    }

    public void Place(Tile tile)
    {
        Tile = tile;
        transform.position = Tile.transform.position + Vector3.up * 0.5f;
    }
}
