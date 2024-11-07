using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopUpgradeItem : ShopItem
{
    [SerializeField]private string name;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI name_Txt;

    protected override void Start()
    {
        base.Start();
        _multipleBuyable = true;
        icon.sprite = itemIcon;
        name_Txt.text = name;
    }

    protected override void BuyAction()
    {
        DataManager.Inst.Data.skillUpgradeTickets++;
        //업그레이드 아이템 플레이어에게 주어짐
        Debug.Log("업그레이드");
    }
}
