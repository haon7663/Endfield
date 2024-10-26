using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    private List<Tile> _tiles;

    [SerializeField] private int tileCount;
    [SerializeField] private float tileInterval;
    
    [SerializeField] private Transform gridParent;
    [SerializeField] private Tile tilePrefab;

    private void Awake()
    {
        GenerateTiles();
    }

    private void GenerateTiles()
    {
        _tiles = new List<Tile>();

        for (var i = 0; i < tileCount; i++)
        {
            var tile = Instantiate(tilePrefab, gridParent);
            tile.transform.position = new Vector3((i - Mathf.FloorToInt((float)tileCount / 2)) * tileInterval, 0);
            tile.Init(i);
            
            _tiles.Add(tile);
        }
    }

    public Tile GetTile(int index)
    {
        return _tiles[Mathf.Clamp(index, 0, tileCount - 1)];
    }
    
    public Tile GetRandomTile()
    {
        return _tiles.Where(t => !t.IsOccupied).ToList().Random();
    }

    public void OnDrawGizmos()
    {
        /*foreach (var tile in _tiles)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(tile.transform.position + new Vector3(0, 0.375f, 1.5f), new Vector3(1, 0f, 3));
        }*/
    }
}