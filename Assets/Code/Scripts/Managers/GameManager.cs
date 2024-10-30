using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    public Unit Player { get; private set; }

    public int maxElixir;
    public float curElixir;

    [SerializeField] private Unit playerPrefab;
    [SerializeField] private Unit enemyPrefab;

    private void Start()
    {
        var playerData = UnitLoader.GetUnitData("Spider");
        var player = Instantiate(playerPrefab);
        player.Init(playerData, GridManager.Inst.GetRandomTile());

        Player = player;
        
        var enemyData = UnitLoader.GetUnitData("Spider");
        var enemy = Instantiate(enemyPrefab);
        enemy.Init(enemyData, GridManager.Inst.GetRandomTile());
    }

    private void Update()
    {
        curElixir += Time.deltaTime;
        curElixir = Mathf.Clamp(curElixir, 0, maxElixir);
    }
}
