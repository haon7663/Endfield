using System;
using UnityEngine;

public class ShopCard : ShopItem
{
    Card card;

    private void Awake()
    {
        _multipleBuyable = false;
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

    protected override void BuyAction()
    {
        //인벤토리에 스킬 넣기
        Debug.Log("스킬 삼");
    }

}
