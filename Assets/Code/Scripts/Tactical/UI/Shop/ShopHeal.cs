using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopHeal : ShopItem
{
    [SerializeField]private string name;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI name_Txt;

    [SerializeField] private float healValue;
    protected override void Start()
    {
        base.Start();
        icon.sprite = itemIcon;
        name_Txt.text = name;
    }

    protected override void BuyAction()
    {
        //플레이어 힐
        Debug.Log("힐 받음");
    }
}
