using UnityEngine;

public class ShopItem : MonoBehaviour
{
    private int itemPrice;
    private bool isSelling;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemPrice = Random.Range(200, 250);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public void BuyItem()
    {
        if (!isSelling) return;
        isSelling = false;
        //플레이어한테서 재화 뺏어가는 코드 들어갈 자리

    }

    public virtual void BuyAction()
    {

    }
}
