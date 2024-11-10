using System;
using UnityEngine;

public class ShopCard : ShopItem
{
    Card card;
    private Skill _skill;
    ShopController _shopController;

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
        card.ShowAnim();
    }

    public void ShopConTrollerInput(ShopController shopController)
    {
        _shopController = shopController;
    }

    protected override void BuyAction()
    {
        _shopController.Hide();
        SkillChangeController.Inst.Show(_skill);
       
        Debug.Log("스킬 삼");
    }

}
