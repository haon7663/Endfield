using System;
using System.Collections;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private Unit playerPrefab;
    [SerializeField] private Unit enemyPrefab;
    [SerializeField] private int maxEnemyCount;
    [SerializeField] private GameObject spawnEffect;
    private int _surviveEnemyCount;

    private void Start()
    {
        _surviveEnemyCount = 1;
    }

    public Unit Spawn(string unitName, bool isPlayer = false)
    {
        var unitData = UnitLoader.GetUnitData(unitName);
        var randomTile = GridManager.Inst.GetRandomTile();
        var unit = Instantiate(isPlayer ? playerPrefab : enemyPrefab);
        unit.Init(unitData, randomTile);
        
        if (!isPlayer)
        {
            unit.gameObject.SetActive(false);
            var effect =  Instantiate(spawnEffect, randomTile.transform.position + Vector3.up * 1f, quaternion.identity);
            DOVirtual.DelayedCall(1, ()=>
            {
                Destroy(effect);
                unit.gameObject.SetActive(true);
            });
        }
        
        return unit;
    }



    public void EnemyDead()
    {
        --_surviveEnemyCount;
        if(_surviveEnemyCount <= 0)
            SpawnEnemy();
    }
    
    private void SpawnEnemy()
    {
        for (int i = 0; i < maxEnemyCount; i++)
        {
            SpawnManager.Inst.Spawn("Spider", false);
            _surviveEnemyCount = maxEnemyCount;
        }
    }
}
