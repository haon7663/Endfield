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
        explanText.text = relicSO.description;
        icon.sprite = relicSO.sprite;
        amountText.text = amount.ToString() + "°³";
    }
}
