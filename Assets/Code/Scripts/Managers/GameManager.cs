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
        Player = SpawnManager.Inst.Get("Player", true);
        SpawnManager.Inst.Get("Spider", false);
    }

    private void Update()
    {
        curElixir += Time.deltaTime;
        curElixir = Mathf.Clamp(curElixir, 0, maxElixir);
    }
}
