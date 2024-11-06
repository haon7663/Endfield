using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ArtifactManager : SingletonDontDestroyOnLoad<ArtifactManager>
{
    public int hpRegenArtifact;
    public int elixirArtifact;
    public int goldArtifact;
    public int maxHpArtifact;
    public int skillUpgradeArtifact;

    [SerializeField] private int reGenHp;
    [SerializeField] private int increaseElixir;
    [SerializeField] private int bonusGold;
    [SerializeField] private int increaseHp;
    [SerializeField] private float skillUpgradeTicketProb;

    public void Artifact() //������ �Ѿ�� �����
    {
        IncreaseHpMax();
        HpRegen();
        IncreaseElixirMax();
        BonusGoldOnClear();
        GetSkillUpgradeTicket();
    }

    private void HpRegen()
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

    private void IncreaseElixirMax()
    {
        if (elixirArtifact > 0)
        {
            int newElixir = GameManager.Inst.maxElixir + elixirArtifact * increaseElixir;
            GameManager.Inst.maxElixir = newElixir;
            Debug.Log("�ִ� ������ ����" + elixirArtifact * increaseElixir);
        }
    }

    private void BonusGoldOnClear()
    {
        if (goldArtifact > 0)
        {
            int gold = DataManager.Inst.Data.gold + goldArtifact * bonusGold;
            DataManager.Inst.Data.gold = gold;
            Debug.Log("��� ȹ��" + goldArtifact * bonusGold);
        }
    }

    private void IncreaseHpMax()
    {
        if (maxHpArtifact > 0)
        {
            int newHp = GameManager.Inst.Player.Health.maxHp + maxHpArtifact * increaseHp;
            GameManager.Inst.Player.Health.maxHp = newHp;
            Debug.Log("�ִ�ü�� ����" + maxHpArtifact * increaseHp);
        }
    }

    private void GetSkillUpgradeTicket()
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
        elixirArtifact = 0;
        goldArtifact = 0;
        maxHpArtifact = 0;
        skillUpgradeArtifact = 0;

    }
}
