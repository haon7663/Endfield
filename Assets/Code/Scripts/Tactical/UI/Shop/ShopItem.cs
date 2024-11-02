using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private int maxPrice, minPrice;
    private int _itemPrice;
    private bool _isSelling = true;
    [SerializeField] private TextMeshProUGUI itemPrice_Txt;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        if (maxPrice < minPrice)(maxPrice, minPrice) = (minPrice, maxPrice); //만약 최솟값, 최댓값 반대로 입력했으면 정상화
        _itemPrice = Random.Range(minPrice, maxPrice);
        itemPrice_Txt.text = _itemPrice.ToString() + " ml";
    }
    public void BuyItem()
    {
        Debug.Log("상점");
        if (!_isSelling || DataManager.Inst.Data.gold < _itemPrice) return;
        
        _isSelling = false;
        itemPrice_Txt.text = "Sold";
        DataManager.Inst.Data.gold -= _itemPrice;
        BuyAction();
    }

    protected virtual void BuyAction(){}
}
