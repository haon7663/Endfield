using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private int maxPrice, minPrice;
    private int _itemPrice;
    private bool _isSelling;
    [SerializeField] private TextMeshProUGUI itemPrice_Txt;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        if (maxPrice < minPrice)(maxPrice, minPrice) = (minPrice, maxPrice); //만약 최솟값, 최댓값 반대로 입력했으면 정상화
        _itemPrice = Random.Range(minPrice, maxPrice);
        itemPrice_Txt.text = "<sprite=0> "+_itemPrice.ToString() + " ml";
    }
    public void BuyItem()
    {
        if (!_isSelling) return;
        _isSelling = false;
        //플레이어 돈 뺏기
        BuyAction();
    }

    public virtual void BuyAction(){}
}
