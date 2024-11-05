using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Unit Player { get; private set; }

    public int maxElixir;
    public float curElixir;
    public float elixirRegenerationSpeed = 0.5f;

    public float startViewPoint;
    
    [SerializeField] private DefeatPanelController defeatController;
    [SerializeField] private SkillSelectionPanelController skillSelectionController;
    [SerializeField] private MapIconController mapIconController;

    private void Awake()
    {
        startViewPoint = DataManager.Inst.Data.stageCount * 9.8f;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => SpawnManager.Inst);
        Player = SpawnManager.Inst.Summon("Player", GridManager.Inst.GetTile(0), true);
    }

    private void Update()
    {
        curElixir += Time.deltaTime * elixirRegenerationSpeed;
        curElixir = Mathf.Clamp(curElixir, 0, maxElixir);
    }

    public void StageEnd(bool isPlayerWin)
    {
        if (isPlayerWin)
        {
            skillSelectionController.Show();
            MapIconShow(true);
        }
        else defeatController.Show();
    }

    public void MapIconShow(bool show) => (show ? (Action)mapIconController.Show : mapIconController.Hide)();
}
