using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ArtifactManager : SingletonDontDestroyOnLoad<ArtifactManager>
{
    public int hpRegenArtifact;
    public int maxElixirArtifact;
    public int goldArtifact;
    public int maxHpArtifact;
    public int skillUpgradeArtifact;
    public List<RelicSO> relics;

    [FormerlySerializedAs("reGenHp")] [SerializeField] private int regenHp;
    [SerializeField] private int increaseElixir;
    [SerializeField] private int bonusGold;
    [SerializeField] private int increaseHp;
    [SerializeField] private float skillUpgradeTicketProb;

    private int baseMaxElixir = 4;
    private int baseMaxHp = 60;

    public enum ArtifactType
    {
        HpRegen,
        MaxHp,
        MaxElixir,
        Gold,
        SkillUpgradeTicket
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
            int newHp = DataManager.Inst.Data.curHp + hpRegenArtifact * regenHp;

            if (newHp >= GameManager.Inst.Player.Health.maxHp)
            {
                DataManager.Inst.Data.curHp = GameManager.Inst.Player.Health.maxHp;
            }
            else
            {
                DataManager.Inst.Data.curHp += hpRegenArtifact * regenHp;
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
            if (Random.value * 100 <= skillUpgradeTicketProb * skillUpgradeArtifact)
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

    public int CalculateArtifactValue(ArtifactType artifactType)
    {
        switch (artifactType)
        {
            case ArtifactType.HpRegen:
                return hpRegenArtifact * regenHp;
            case ArtifactType.MaxHp:
                return maxHpArtifact * increaseHp;
            case ArtifactType.MaxElixir:
                return maxElixirArtifact * increaseElixir;
            case ArtifactType.Gold:
                return goldArtifact * bonusGold;
            case ArtifactType.SkillUpgradeTicket:
                return (int)(skillUpgradeTicketProb * skillUpgradeArtifact );
            default:
                return 0;
        }
    }
    
    public int CalculateArtifactValue(ArtifactType artifactType,int amount)
    {
        switch (artifactType)
        {
            case ArtifactType.HpRegen:
                return amount * regenHp;
            case ArtifactType.MaxHp:
                return amount * increaseHp;
            case ArtifactType.MaxElixir:
                return amount* increaseElixir;
            case ArtifactType.Gold:
                return amount * bonusGold;
            case ArtifactType.SkillUpgradeTicket:
                return amount * skillUpgradeArtifact;
            default:
                return 0;
        }
    }
}
