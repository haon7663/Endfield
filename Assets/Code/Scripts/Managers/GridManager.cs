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

    [SerializeField] private Color playerColor;
    [SerializeField] private Color enemyColor;
    private Dictionary<Unit, List<Tile>> _displayedTiles = new Dictionary<Unit, List<Tile>>();

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

    public void RevertAllGrid()
    {
        foreach (var tile in _tiles)
        {
            tile.SetColor(Color.white);
        }
    }
    
    public void DisplayGrid(Unit user, List<Tile> tiles)
    {
        _displayedTiles.Add(user, tiles);
        ResetTilesColor();
    }

    public void RevertGrid(Unit user)
    {
        if (_displayedTiles.ContainsKey(user))
            _displayedTiles.Remove(user);

        ResetTilesColor();
    }

    private void ResetTilesColor()
    {
        RevertAllGrid();
        foreach (var displayedTile in _displayedTiles)
        {
            var color = displayedTile.Key.unitType == UnitType.Player ? playerColor : enemyColor;
            displayedTile.Value.ForEach(t => t.SetColor(color));
        }
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