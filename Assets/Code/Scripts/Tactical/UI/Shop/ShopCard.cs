using System;
using UnityEngine;

public class ShopCard : ShopItem
{
    Card card;

    private void Awake()
    {
        card = GetComponent<Card>();
    }

    protected override void Start()
    {
        base.Start();
        
    }
    public void RandomCardInput(Skill skill)
    {
        Debug.Log(skill.name);
        card.Init(skill);
    }

    public override void BuyAction()
    {
        //인벤토리에 스킬 넣기
    }

}
