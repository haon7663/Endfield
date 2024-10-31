using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private Unit playerPrefab;
    [SerializeField] private Unit enemyPrefab;
    [SerializeField] private int maxEnemyCount;
    [SerializeField] private UnitSpawnHandler spawnHandlerPrefab;
    private int _surviveEnemyCount;

    private void Start()
    {
        _surviveEnemyCount = 0;
        SpawnEnemy();
    }

    public Unit Summon(string unitName, Tile tile, bool isPlayer = false)
    {
        var unitData = UnitLoader.GetUnitData(unitName);
        var unit = Instantiate(isPlayer ? playerPrefab : enemyPrefab);
        unit.Init(unitData, tile);

        return unit;
    }

    public void SpawnEnemy(string unitName, Tile tile = null)
    {
        var targetTile = tile ? tile : GridManager.Inst.GetRandomTile();
        
        var spawnHandler = Instantiate(spawnHandlerPrefab);
        spawnHandler.Init(unitName, targetTile);
    }

    public void EnemyDead()
    {
        if(--_surviveEnemyCount <= 0)
            SpawnEnemy();
    }
    
    private void SpawnEnemy()
    {
        var tiles = new List<Tile>();
        for (var i = 0; i < maxEnemyCount; i++)
        {
            var tile = GridManager.Inst.GetRandomTile(tiles);
            SpawnEnemy("Spider", tile);
            tiles.Add(tile);
            _surviveEnemyCount++;
        }
    }
}
