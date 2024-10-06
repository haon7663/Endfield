using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static Dictionary<Vector2, Tile> Tiles { get; private set; }

    [SerializeField] private Transform grid;
    [SerializeField] private Transform gridParent;
    [SerializeField] private Tile tilePrefab;

    private void Awake()
    {
        GenerateTiles();
    }

    private void GenerateTiles()
    {
        Tiles = new Dictionary<Vector2, Tile>();
        
        var tileMaps = grid.GetComponentsInChildren<Tilemap>().OrderByDescending(x => x.GetComponent<TilemapRenderer>().sortingOrder);
        foreach (var tileMap in tileMaps)
        {
            var bounds = tileMap.cellBounds;

            for (var x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (var y = bounds.yMin; y < bounds.yMax; y++)
                {
                    if (!tileMap.HasTile(new Vector3Int(x, y, 0))) continue;
                    
                    var tileKey = new Vector2(x, y);
                    
                    if (Tiles.ContainsKey(tileKey)) continue;
                    
                    var cellWorldPosition = tileMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    
                    var tile = Instantiate(tilePrefab, gridParent);
                    tile.transform.position = cellWorldPosition;
                    tile.Init(tileKey);
                    
                    Tiles.Add(tileKey, tile);
                }
            }
        }
        //foreach (var tile in Tiles.Values) tile.CacheNeighbors();
    }
}