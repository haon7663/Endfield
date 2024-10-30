using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private Unit playerPrefab;
    [SerializeField] private Unit enemyPrefab;

    public Unit Get(string unitName, bool isPlayer = false)
    {
        var unitData = UnitLoader.GetUnitData(unitName);
        var unit = Instantiate(isPlayer ? playerPrefab : enemyPrefab);
        unit.Init(unitData, GridManager.Inst.GetRandomTile());
        return unit;
    }
}
