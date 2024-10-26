using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillSelectionPanelController : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform cardGroup;

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
    }

    private void GetSkill(Skill skill)
    {
        DataManager.Inst.Data.skills.Add(skill);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
