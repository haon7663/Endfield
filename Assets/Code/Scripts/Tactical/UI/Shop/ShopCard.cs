using UnityEngine;

public class ShopCard : ShopItem
{
    Card card;
    [SerializeField] private Skill skillData;

    private void Start()
    {
        card = GetComponent<Card>();
    }
    public void RandomCardInput()
    {
        //카드 랜덤으로 들어가는 코드
        card.Init(skillData);
       
    }

    public override void BuyAction()
    {
        //인벤토리에 스킬 이동
    }

}
