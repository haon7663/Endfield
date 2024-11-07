using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class InventoryRelicInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI explainText;
    [SerializeField] private Image icon;
    
    public void Init(RelicSO relic, int amount)
    {
        nameLabel.text = relic.name;
        icon.sprite = relic.sprite;
        explainText.text = GetExplainText(relic, amount);
    }

    private string GetExplainText(RelicSO relic, int amount)
    {
        return relic.relicType switch
        {
            RelicSO.RelicType.GetGold => $"스테이지 시작마다 {amount}ml를 획득합니다.",
            RelicSO.RelicType.MaxElixier => $"엘릭서 최대량이 {amount} 증가합니다.",
            RelicSO.RelicType.MaxHp => $"최대 체력이 {amount} 증가합니다.",
            RelicSO.RelicType.GetSkillUpgrade => $"스테이지 종료 시 {amount}% 확률로 스킬 강화권을 1개 획득합니다",
            RelicSO.RelicType.HpRegen => $"스테이지 시작 시 체력을 {amount} 회복합니다.",
            _ => explainText.text
        };
    }
}