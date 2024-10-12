using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static List<Tile> Tiles { get; private set; }

    [SerializeField] private int tileCount;
    
    [SerializeField] private Transform gridParent;
    [SerializeField] private Tile tilePrefab;

    private void Awake()
    {
        GenerateTiles();
    }

    private void GenerateTiles()
    {
        Tiles = new List<Tile>();

        for (var i = 0; i < tileCount; i++)
        {
            var tile = Instantiate(tilePrefab, gridParent);
            tile.transform.position = new Vector3(i - tileCount / 2, 0);
            tile.Init(i);
            
            Tiles.Add(tile);
        }
    }
}