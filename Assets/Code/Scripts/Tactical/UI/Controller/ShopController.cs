using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopController : MonoBehaviour
{
    [SerializeField] private Panel panel;
    [SerializeField] private ClosePanel closePanel;
    [SerializeField] private Transform sellCards;
    [SerializeField]private List<ShopCard> _cards = new List<ShopCard>();

    private void Awake()
    {
        for (int i = 0; i < sellCards.childCount; i++)
        {
            _cards.Add(sellCards.GetChild(i).GetComponent<ShopCard>());
        }
       
    }
    
    
    public void Show()
    {
        CardInfo();
        closePanel.onClose += Hide;
        panel.SetPosition(PanelStates.Show, true);
    }

    public void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true);
        closePanel.onClose -= Hide;
    }
    

    private void CardInfo()
    {
        var skills = SkillLoader.GetAllSkills("skill");
        var haveSkills = new List<Skill>();
        haveSkills.AddRange(DataManager.Inst.Data.skills);
        for (var i = 0; i < 3; i++)
        {
            var skill = skills.Where(s => !haveSkills.Contains(s)).ToList().Random();
            haveSkills.Add(skill);
            _cards[i].RandomCardInput(skill);

        }
    }
}
