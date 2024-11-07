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
            explanText.text = "스테이지 시작마다 " + amount.ToString() + "ml를 획득합니다.";
        }
        else if (relicSO.relicType == RelicSO.RelicType.MaxElixier)
        {
            explanText.text = "엘릭서 최대량이 " + amount.ToString() + " 증가합니다.";
        }
        else if (relicSO.relicType == RelicSO.RelicType.MaxHp)
        {
            explanText.text = "최대 체력이 " + amount.ToString() + " 증가합니다.";
        }
        else if (relicSO.relicType == RelicSO.RelicType.GetSkillUpgrade)
        {
            explanText.text = "스테이지 종료 시 " + amount.ToString() + "% 확률로 스킬 강화권을 1개 획득합니다";
        }
        else if (relicSO.relicType == RelicSO.RelicType.HpRegen)
        {
            explanText.text = "스테이지 시작 시 체력을 " + amount.ToString() + " 회복합니다.";
        }

    }
}