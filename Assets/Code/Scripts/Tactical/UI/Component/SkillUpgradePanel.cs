using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class SkillUpgradePanel : MonoBehaviour
{
    [SerializeField] private Panel panel;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform cardGroup;
    [SerializeField] private ClosePanel closePanel;

    public void Show(Skill skill)
    {
        var skillComponents = SkillLoader.GetAllSkillComponent("upgradeSkill");
        var haveComponents = new List<SkillComponent>();
        for (var i = 0; i < 3; i++)
        {
            var skillComponent = skillComponents.Where(s => !haveComponents.Contains(s)).ToList().Random();
            haveComponents.Add(skillComponent);
            
            Debug.Log(skillComponent.subDescription);

            var newSkill = new Skill(skill.name)
            {
                label = skill.label,
                description = skill.description,
                elixir = skill.elixir,
                castingTime = skill.castingTime,
                skillComponents = skill.skillComponents.ToList()
            };
            newSkill.skillComponents.Add(skillComponent);
            
            var card = Instantiate(cardPrefab, cardGroup);
            card.Init(newSkill);
            card.onClick += Hide;
            card.onClick += () => UpgradeSkill(skill, newSkill);
        }
        
        panel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
        closePanel.onClose += Hide;
    }

    private void UpgradeSkill(Skill skill, Skill newSkill)
    {
        var playerSkills = DataManager.Inst.Data.skills;
        var index = playerSkills.FindIndex(s => s == skill);
        playerSkills[index] = newSkill;

        foreach (var skillComponent in DataManager.Inst.Data.skills[index].skillComponents)
        {
            Debug.Log(skillComponent.saveName);
        }
    }
    
    public void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true, 0.25f);
    }
}
