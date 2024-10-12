using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Tile Tile { get; private set; }

    private void Start()
    {
        Place(GridManager.Tiles[0]);
    }

    public void Place(Tile tile)
    {
        Tile = tile;
        transform.position = Tile.transform.position + Vector3.up * 0.5f;
    }
}
