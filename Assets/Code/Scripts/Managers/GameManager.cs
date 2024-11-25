using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Unit Player { get; private set; }

    public bool isGameActive;

    public int maxElixir;
    public float curElixir;
    public float elixirRegenerationSpeed = 1f;

    public float startViewPoint;
    
    [SerializeField] private DefeatPanelController defeatController;
    [SerializeField] private SkillSelectionPanelController skillSelectionController;
    [SerializeField] private MapIconController mapIconController;

    private void Awake()
    {
        startViewPoint = DataManager.Inst.Data.stageCount * 7f;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => SpawnManager.Inst);
        Player = SpawnManager.Inst.Summon("Player", GridManager.Inst.GetTile(0), true);

        yield return new WaitForSeconds(0.25f);

        isGameActive = true;
    }

    private void Update()
    {
        maxElixir = DataManager.Inst.Data.maxElixir;
        curElixir += Time.deltaTime * elixirRegenerationSpeed;
        curElixir = Mathf.Clamp(curElixir, 0, maxElixir);

        if (Input.GetKeyDown(KeyCode.F1)) //튜토리얼 스킵
        {
            if (DataManager.Inst.Data.stageCount < 2)
            {
                DataManager.Inst.Data.stageCount = 2;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
           
        }
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
