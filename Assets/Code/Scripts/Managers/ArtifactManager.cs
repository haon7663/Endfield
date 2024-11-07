using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArtifactManager : SingletonDontDestroyOnLoad<ArtifactManager>
{
    public int hpRegenArtifact;
    public int maxElixirArtifact;
    public int goldArtifact;
    public int maxHpArtifact;
    public int skillUpgradeArtifact;
    public List<RelicSO> relics  = new List<RelicSO>();

    [SerializeField] private int reGenHp;
    [SerializeField] private int increaseElixir;
    [SerializeField] private int bonusGold;
    [SerializeField] private int increaseHp;
    [SerializeField] private float skillUpgradeTicketProb;

    private int baseMaxElixir = 4;
    private int baseMaxHp = 25;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            IncreaseHpMax();
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            hpRegenArtifact++;
        }
    }

    public void ArtifactForStage() 
    {
        ResetMaxHp();
        HpRegen();
        BonusGoldOnClear();
        GetSkillUpgradeTicket();
    }

    public void HpRegen()
    {
        if (hpRegenArtifact > 0)
        {
            int newHp = DataManager.Inst.Data.curHp + hpRegenArtifact * reGenHp;

            if (newHp >= GameManager.Inst.Player.Health.maxHp)
            {
                DataManager.Inst.Data.curHp = GameManager.Inst.Player.Health.maxHp;
            }
            else
            {
                DataManager.Inst.Data.curHp += hpRegenArtifact * reGenHp;
            }
        }
    }

    public void IncreaseElixirMax()
    {
        maxElixirArtifact++;
        if (maxElixirArtifact > 0)
        {
            int increaseAmount = maxElixirArtifact * increaseElixir;
            DataManager.Inst.Data.maxElixir = baseMaxElixir + increaseAmount;  
        }
    }


    public void BonusGoldOnClear()
    {
        if (goldArtifact > 0)
        {
            int gold = DataManager.Inst.Data.gold + goldArtifact * bonusGold;
            DataManager.Inst.Data.gold = gold;
        }
    }

    public void IncreaseHpMax()
    {
        maxHpArtifact++;
        if (maxHpArtifact > 0)
        {
            int increaseAmount = maxHpArtifact * increaseHp;
            GameManager.Inst.Player.Health.maxHp = baseMaxHp + increaseAmount;
            DataManager.Inst.Data.curHp = baseMaxHp + increaseAmount;
            GameManager.Inst.Player.Health.curHp = DataManager.Inst.Data.curHp;
            HealthTextController.Inst.UpdateUI(GameManager.Inst.Player.Health);
        }
    }


    public void GetSkillUpgradeTicket()
    {
        if (skillUpgradeArtifact > 0)
        {
            if (Random.value <= skillUpgradeTicketProb * skillUpgradeArtifact)
            {
                DataManager.Inst.Data.skillUpgradeTickets++;
            }
        }
    }

    public void ResetArtifact()
    {
        hpRegenArtifact = 0;
        maxElixirArtifact = 0;
        goldArtifact = 0;
        maxHpArtifact = 0;
        skillUpgradeArtifact = 0;

    }

    public int ArtifactCount()
    {
        int allArtifact = hpRegenArtifact + maxElixirArtifact
            + goldArtifact + skillUpgradeArtifact + maxHpArtifact;
        return allArtifact;
    }

    public List<RelicSO> GetAllRelics()
    {
        return relics;
    }

    public void ResetMaxHp()
    {
        int increaseAmount = maxHpArtifact * increaseHp;
        GameManager.Inst.Player.Health.maxHp = baseMaxHp + increaseAmount;
        HealthTextController.Inst.UpdateUI(GameManager.Inst.Player.Health);
    }
}
