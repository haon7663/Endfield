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

            var newSkill = SkillUpgrader(skill, skillComponent);
            
            var card = Instantiate(cardPrefab, cardGroup);
            card.Init(newSkill);
            card.onClick += Hide;
            card.onClick += () => UpgradeSkill(skill, skillComponent);
        }
        
        panel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
        closePanel.onClose += Hide;
    }

    private void UpgradeSkill(Skill skill, SkillComponent skillComponent)
    {
        var playerSkills = DataManager.Inst.Data.skills;
        var index = playerSkills.FindIndex(s => s == skill);
        switch (skillComponent.ExecuteType)
        {
            case SkillExecuteType.AddModifier:
                skillComponent.ApplyModify(skill);
                foreach (var component in skill.skillComponents)
                    skillComponent.UpdateModify(component);
                break;
            case SkillExecuteType.MultiplyModifier:
                skillComponent.ApplyModify(skill);
                foreach (var component in skill.skillComponents)
                    skillComponent.UpdateModify(component);
                break;
        }
        playerSkills[index] = SkillUpgrader(skill, skillComponent);
    }

    private Skill SkillUpgrader(Skill skill, SkillComponent skillComponent)
    {
        var newSkill = new Skill(skill.name)
        {
            label = skill.label,
            description = skill.description,
            elixir = skill.elixir,
            castingTime = skill.castingTime,
            executeCount = skill.executeCount,
            skillComponents = skill.skillComponents.ToList()
        };
        newSkill.skillComponents.Add(skillComponent);

        return newSkill;
    }
    
    public void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true, 0.25f);
    }
}
