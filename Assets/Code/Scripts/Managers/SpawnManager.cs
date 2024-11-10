using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private Unit playerPrefab;
    [SerializeField] private Unit enemyPrefab;
    [SerializeField] private UnitSpawnHandler spawnHandlerPrefab;

    [SerializeField] private AnimationCurve levelCurve;
    
    public List<WaveInfo> enemyCounts;
    
    public bool isSpawnEnemy = true;

    private int _stageGold;
    private int _surviveEnemyCount;
    private int _curWaveCount;

    private void Awake()
    {
        Reset();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => DataManager.Inst);
        WaveController.Inst.UpdateWaveText(_curWaveCount);
    }
    
    public Unit Summon(string unitName, Tile tile, bool isPlayer = false)
    {
        var unitData = UnitLoader.GetUnitData(unitName);
        if (unitData == null)
            return null;
        
        var unit = Instantiate(isPlayer ? playerPrefab : enemyPrefab);
        if (!isPlayer)
            unitData.health = Mathf.FloorToInt(unitData.health * levelCurve.Evaluate((float)DataManager.Inst.Data.stageCount / 13));
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
        {
            if (_curWaveCount >= enemyCounts[DataManager.Inst.Data.stageCount].waveCount)
            {
                GameManager.Inst.StageEnd(true);
                GoldController.Inst.ReCountGold(+ _stageGold);
                return;
            }
            SpawnEnemies();
            WaveController.Inst.UpdateWaveText(++_curWaveCount);
        }
    }

    public void Reset()
    {
        _stageGold = Random.Range(150, 180);
        _surviveEnemyCount = 0;
        _curWaveCount = 1;
    }

    public void SpawnEnemies()
    {
        if (!isSpawnEnemy) return;

        var spawnedEnemiesName = new List<string>();
        var spawnedTiles = new List<Tile>();

        var spawnCount = enemyCounts[DataManager.Inst.Data.stageCount].EnemyCount;
        
        for (var i = 0; i < spawnCount; i++)
        {
            var tile = GridManager.Inst.GetRandomTile(spawnedTiles);
            var ableUnits = UnitLoader.GetAllUnitData().Where(unit => unit.name != "Double Flower" && spawnedEnemiesName.Count(e => unit.name == e) < spawnCount).ToList();
            var targetUnitName = ableUnits.Random().name;

            SpawnEnemy(targetUnitName, tile);
            spawnedEnemiesName.Add(targetUnitName);
            spawnedTiles.Add(tile);
            _surviveEnemyCount++;
        }
    }
}

[Serializable]
public struct WaveInfo
{
    public int minEnemyCount;
    public int maxEnemyCount;
    public int waveCount;

    public int EnemyCount => Random.Range(minEnemyCount, maxEnemyCount + 1);
}