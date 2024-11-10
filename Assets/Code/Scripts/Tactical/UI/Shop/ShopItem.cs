using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int maxPrice, minPrice;
    protected bool _multipleBuyable;
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
        GoldController.Inst.ReCountGold( - _itemPrice);
        
        SoundManager.Inst.Play("ShopEffect");
        BuyAction();
        _isSelling = false;
        if (!_multipleBuyable) itemPrice_Txt.text = "Sold";
        else
        {
            float value = 1f;
            DOTween.To(() => value, x => value = x, 0, 1.5f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                value = Mathf.Round(value * 10) / 10;
                itemPrice_Txt.text = value.ToString();
            }).OnComplete(()=>
            {
                _isSelling = true;
                itemPrice_Txt.text = _itemPrice.ToString() + " ml";
            });
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BuyItem();
    }

    protected virtual void BuyAction(){}
}
