using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class InventoryRelicInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI explanText;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image icon;


    public void InitRelicInfo(RelicSO relicSO, int amount)
    {
        nameLabel.text = relicSO.name;
        icon.sprite = relicSO.sprite;
        if(relicSO.relicType == RelicSO.RelicType.GetGold)
        {
            explanText.text = "�������� ���۸��� " + amount.ToString() + "ml�� ȹ���մϴ�.";
        }
        else if (relicSO.relicType == RelicSO.RelicType.MaxElixier)
        {
            explanText.text = "������ �ִ뷮�� " + amount.ToString() + " �����մϴ�.";
        }
        else if (relicSO.relicType == RelicSO.RelicType.MaxHp)
        {
            explanText.text = "�ִ� ü���� " + amount.ToString() + " �����մϴ�.";
        }
        else if (relicSO.relicType == RelicSO.RelicType.GetSkillUpgrade)
        {
            explanText.text = "�������� ���� �� " + amount.ToString() + "% Ȯ���� ��ų ��ȭ���� 1�� ȹ���մϴ�";
        }
        else if (relicSO.relicType == RelicSO.RelicType.HpRegen)
        {
            explanText.text = "�������� ���� �� ü���� " + amount.ToString() + " ȸ���մϴ�.";
        }

    }
}