using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    private List<Tile> _tiles;

    [SerializeField] private int tileCount;
    [SerializeField] private float tileInterval;

    [SerializeField] private Transform gridParent;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Tile transitionTilePrefab;

    [SerializeField] private PreviewSprite previewSpritePrefab;
    [SerializeField] private TMP_Text previewTextPrefab;

    [SerializeField] private Color playerColor;
    [SerializeField] private Color enemyColor;

    private Tile _transition;
    private bool _isTransitioning;

    private Dictionary<Unit, List<Tile>> _previewTiles = new Dictionary<Unit, List<Tile>>();

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Inst);
        GenerateTiles();
    }

    private void Update()
    {
        CheckingTransition();
    }

    private void GenerateTiles()
    {
        _tiles = new List<Tile>();

        for (var i = 0; i < tileCount; i++)
        {
            var tile = Instantiate(tilePrefab, gridParent);
            tile.transform.position = GetTilePosition(i);
            tile.Init(i);

            _tiles.Add(tile);
        }
    }

    public void GenerateTransitionTiles()
    {
        var tile = Instantiate(transitionTilePrefab, gridParent);
        tile.transform.position = GetTilePosition(tileCount);
        tile.Init(tileCount);
        _transition = tile;

        _isTransitioning = true;
        _tiles.Add(tile);
    }

    private Vector3 GetTilePosition(int index)
    {
        return new Vector3((index - Mathf.FloorToInt((float)tileCount / 2)) * tileInterval + GameManager.Inst.startViewPoint, 0);
    }

    public Tile GetTile(int index)
    {
        return _tiles[Mathf.Clamp(index, 0, _tiles.Count - 1)];
    }
    
    public bool TryGetTile(int index, out Tile tile)
    {
        var success = (uint)index < (uint)_tiles.Count;
        tile = success ? _tiles[index] : null;
        return success;
    }

    public Tile GetRandomTile(List<Tile> exceptionTiles = null)
    {
        var tiles = _tiles;
        if (exceptionTiles != null)
            tiles = _tiles.Where(t => !exceptionTiles.Contains(t)).ToList();
        return tiles.Where(t => !t.IsOccupied).ToList().Random();
    }

    public Tile FindNearestTile(int index)
    {
        var moveAbleTiles = _tiles.Where(t => !t.IsOccupied).ToList();
        var nearestKey = moveAbleTiles.Min(t => Mathf.Abs(t.Key - index));
        return moveAbleTiles.Where(t => Mathf.Abs(t.Key - index) == nearestKey).ToList().Random();
    }

    public void RevertAllGrid()
    {
        foreach (var tile in _tiles)
        {
            tile.SetDefaultColor();
        }
    }

    public void UpdateContentOnGrid()
    {

    }

    public PreviewSprite DisplayPreview(Unit user, int key)
    {
        var tile = GetTile(key);
        var previewSprite = Instantiate(previewSpritePrefab, tile.transform.position, Quaternion.identity);
        previewSprite.Init(tile.Key, user.Renderer.sprite);

        return previewSprite;
    }

    public void ApplyGrid(Unit user, List<Tile> tiles)
    {
        if (!_previewTiles.TryAdd(user, tiles))
            _previewTiles[user].AddRange(tiles);
        UpdateGrid();
    }
    public void RevertGrid(Unit user)
    {
        if (_previewTiles.ContainsKey(user))
            _previewTiles.Remove(user);
        UpdateGrid();
    }
    private void UpdateGrid()
    {
        RevertAllGrid();
        foreach (var displayedTile in _previewTiles)
        {
            var color = displayedTile.Key.unitType == UnitType.Player ? playerColor : enemyColor;
            displayedTile.Value.ForEach(t => t.SetColor(color));
        }
    }

    private void CheckingTransition()
    {
        if(_transition != null)
        {
            if (_transition.content != null && _isTransitioning)
            {
                CameraTransition.Inst.CameraUp();
                GameManager.Inst.MapIconShow(false);
                _isTransitioning = false;
            }
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