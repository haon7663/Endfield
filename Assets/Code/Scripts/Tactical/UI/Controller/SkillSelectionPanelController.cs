using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class SkillSelectionPanelController : MonoBehaviour
{
    [SerializeField] private Panel panel;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform cardGroup;
    [SerializeField] private ClosePanel closePanel;

    private void Start()
    {
        Show();
    }

    public void Show()
    {
        var skills = SkillLoader.GetSkills("skill");
        var haveSkills = new List<Skill>();
        haveSkills.AddRange(DataManager.Inst.Data.skills);
        for (var i = 0; i < 3; i++)
        {
            var skill = skills.Where(s => !haveSkills.Contains(s)).ToList().Random();
            haveSkills.Add(skill);
            
            var card = Instantiate(cardPrefab, cardGroup);
            card.Init(skill);
            card.onClick += Hide;
            card.onClick += () => GetSkill(skill);
        }
        
        panel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
        closePanel.onClose += Hide;
    }

    private void GetSkill(Skill skill)
    {
        DataManager.Inst.Data.skills.Add(skill);
    }
    
    public void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true, 0.25f);
    }
}
