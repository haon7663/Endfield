using System;
using UnityEngine;

public class ShopCard : ShopItem
{
    Card card;
    private Skill _skill;

    private void Awake()
    {
        card = GetComponent<Card>();
        _multipleBuyable = false;
       
    }

    protected override void Start()
    {
        base.Start();
        
    }
    public void RandomCardInput(Skill skill)
    {
        _skill = skill;
        Debug.Log(skill.name);
        card.Init(skill);
    }

    protected override void BuyAction()
    {
        DataManager.Inst.Data.skills.Add(_skill);
        Debug.Log("스킬 삼");
    }

}
