using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ArtifactManager : SingletonDontDestroyOnLoad<ArtifactManager>
{
    public int hpRegenArtifact;
    public int maxElixirArtifact;
    public int goldArtifact;
    public int maxHpArtifact;
    public int skillUpgradeArtifact;

    [SerializeField] private int reGenHp;
    [SerializeField] private int increaseElixir;
    [SerializeField] private int bonusGold;
    [SerializeField] private int increaseHp;
    [SerializeField] private float skillUpgradeTicketProb;

    private int baseMaxElixir = 6;
    private int baseMaxHp = 25;

     /* 체력 리젠, 보너스 골드, 강화권 유물은 변수값 올려주면 적용됨. 
      * 최대 체력, 엘릭서 증가 유물은 밑에 함수 실행할 때 마다 적용됨*/

    public void ArtifactForStage() //스테이지 넘어갈 때 마다 자동 적용됨
    {
        HpRegen();
        BonusGoldOnClear();
        GetSkillUpgradeTicket();
    }

    public void HpRegen()
    {
        if (hpRegenArtifact > 0)
        {
            int newHp = DataManager.Inst.Data.curHp + hpRegenArtifact * reGenHp;

            if (newHp > GameManager.Inst.Player.Health.maxHp)
            {
                DataManager.Inst.Data.curHp = GameManager.Inst.Player.Health.maxHp;
                Debug.Log("풀피로 회복됨");
            }
            else
            {
                DataManager.Inst.Data.curHp = newHp;
                Debug.Log("회복됨" + hpRegenArtifact * reGenHp);
            }
        }
    }

    public void IncreaseElixirMax() //따로 함수 호출해줘야함
    {
        maxElixirArtifact++;
        if (maxElixirArtifact > 0)
        {
            int increaseAmount = maxElixirArtifact * increaseElixir;
            GameManager.Inst.maxElixir = baseMaxElixir + increaseAmount;  
            Debug.Log("최대 엘릭서 증가: " + increaseAmount);
        }
    }


    public void BonusGoldOnClear()
    {
        if (goldArtifact > 0)
        {
            int gold = DataManager.Inst.Data.gold + goldArtifact * bonusGold;
            DataManager.Inst.Data.gold = gold;
            Debug.Log("골드 획득" + goldArtifact * bonusGold);
        }
    }

    public void IncreaseHpMax() //따로 함수 호출해줘야함
    {
        maxHpArtifact++;
        if (maxHpArtifact > 0)
        {
            int increaseAmount = maxHpArtifact * increaseHp;
            GameManager.Inst.Player.Health.maxHp = baseMaxHp + increaseAmount;
            Debug.Log("최대 체력 증가: " + increaseAmount);
        }
    }


    public void GetSkillUpgradeTicket()
    {
        if (skillUpgradeArtifact > 0)
        {
            if (Random.value <= skillUpgradeTicketProb * skillUpgradeArtifact)
            {
                DataManager.Inst.Data.skillUpgradeTickets++;
                Debug.Log("티켓 획득" + DataManager.Inst.Data.skillUpgradeTickets);
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
}
