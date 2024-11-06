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

    private int baseMaxElixir = 6;
    private int baseMaxHp = 25;

     /* ü�� ����, ���ʽ� ���, ��ȭ�� ������ ������ �÷��ָ� �����. 
      * �ִ� ü��, ������ ���� ������ �ؿ� �Լ� ������ �� ���� �����*/

    public void ArtifactForStage() //�������� �Ѿ �� ���� �ڵ� �����
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
                Debug.Log("Ǯ�Ƿ� ȸ����");
            }
            else
            {
                DataManager.Inst.Data.curHp = newHp;
                Debug.Log("ȸ����" + hpRegenArtifact * reGenHp);
            }
        }
    }

    public void IncreaseElixirMax() //���� �Լ� ȣ���������
    {
        maxElixirArtifact++;
        if (maxElixirArtifact > 0)
        {
            int increaseAmount = maxElixirArtifact * increaseElixir;
            GameManager.Inst.maxElixir = baseMaxElixir + increaseAmount;  
            Debug.Log("�ִ� ������ ����: " + increaseAmount);
        }
    }


    public void BonusGoldOnClear()
    {
        if (goldArtifact > 0)
        {
            int gold = DataManager.Inst.Data.gold + goldArtifact * bonusGold;
            DataManager.Inst.Data.gold = gold;
            Debug.Log("��� ȹ��" + goldArtifact * bonusGold);
        }
    }

    public void IncreaseHpMax() //���� �Լ� ȣ���������
    {
        maxHpArtifact++;
        if (maxHpArtifact > 0)
        {
            int increaseAmount = maxHpArtifact * increaseHp;
            GameManager.Inst.Player.Health.maxHp = baseMaxHp + increaseAmount;
            Debug.Log("�ִ� ü�� ����: " + increaseAmount);
        }
    }


    public void GetSkillUpgradeTicket()
    {
        if (skillUpgradeArtifact > 0)
        {
            if (Random.value <= skillUpgradeTicketProb * skillUpgradeArtifact)
            {
                DataManager.Inst.Data.skillUpgradeTickets++;
                Debug.Log("Ƽ�� ȹ��" + DataManager.Inst.Data.skillUpgradeTickets);
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
}
