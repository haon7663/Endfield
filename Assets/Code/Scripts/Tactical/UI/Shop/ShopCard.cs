using UnityEngine;

public class ShopCard : ShopItem
{
    Card card;
    [SerializeField] private Skill skillData;

    protected override void Start()
    {
        base.Start();
        card = GetComponent<Card>();
    }
    public void RandomCardInput()
    {
        skillData = new Skill("Slash");
        card.Init(skillData);
       
    }

    public override void BuyAction()
    {
        //인벤토리에 스킬 넣기
    }

}
