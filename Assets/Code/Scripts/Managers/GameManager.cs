using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    public Unit Player { get; private set; }

    public int maxElixir;
    public float curElixir;
    [SerializeField] private DefeatPanelController defeatController;
    [SerializeField] private SkillSelectionPanelController skillSelectionController;

    private void Start()
    {
        Player = SpawnManager.Inst.Summon("Player", GridManager.Inst.GetTile(4), true);
    }

    private void Update()
    {
        curElixir += Time.deltaTime;
        curElixir = Mathf.Clamp(curElixir, 0, maxElixir);
    }
    

    public void StageEnd(bool isPlayerWin)
    {
        if (isPlayerWin) skillSelectionController.Show();
        else defeatController.Show();
    }
}
