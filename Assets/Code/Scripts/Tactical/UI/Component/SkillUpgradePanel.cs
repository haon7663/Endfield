using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class SkillUpgradePanel : MonoBehaviour
{
    [SerializeField] private Panel panel;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform cardGroup;
    [SerializeField] private ClosePanel closePanel;
    
    [SerializeField] private SkillUpgradeGroup skillUpgradeGroup;

    private List<Card> cards = new List<Card>();

    private int _saveIndex;

    public void Show(Skill skill)
    {
        cards = new List<Card>();
        _saveIndex = DataManager.Inst.Data.skills.FindIndex(s => s == skill);
        
        var skillComponents = SkillLoader.GetAllSkillComponent("upgradeSkill");
        var arrangedSkillComponents = new List<List<SkillComponent>>();

        var saveName = "";
        foreach (var skillComponent in skillComponents)
        {
            if (string.IsNullOrEmpty(skillComponent.saveName) || skillComponent.saveName == saveName)
            {
                skillComponent.saveName = null;
                arrangedSkillComponents.Last().Add(skillComponent);
            }
            else
            {
                arrangedSkillComponents.Add(new List<SkillComponent> { skillComponent });
                saveName = skillComponent.saveName;
            }
        }
        
        var haveComponents = new List<List<SkillComponent>>();
        for (var i = 0; i < 3; i++)
        {
            var skillComponent = arrangedSkillComponents
                .Where(s => !haveComponents.Contains(s) && IsUseAbleComponent(skill, s[0])).ToList().Random();
            haveComponents.Add(skillComponent);
            
            var newSkill = GetUpgradeSkill(skill, skillComponent);
            
            var card = Instantiate(cardPrefab, cardGroup);
            card.Init(newSkill);
            card.onClick += Hide;
            card.onClick += () => UpgradeSkill(newSkill, skillComponent);
            
            cards.Add(card);
        }
        
        panel.SetPosition(PanelStates.Show, true, 0.3f, Ease.OutBack);
        closePanel.onClose += Hide;
    }

    private bool IsUseAbleComponent(Skill skill, SkillComponent component)
    {
        if (string.IsNullOrEmpty(component.useAbleComponent))
            return true;

        return component.useAbleComponent switch
        {
            "Attack" => skill.baseComponent == "Attack",
            _ => true
        };
    }

    private void UpgradeSkill(Skill skill, List<SkillComponent> skillComponents)
    {
        if (DataManager.Inst.Data.skillUpgradeTickets < skill.upgradeCount + 1) return;

        foreach (var skillComponent in skillComponents)
        {
            switch (skillComponent.ExecuteType)
            {
                case SkillExecuteType.AddModifier:
                    skillComponent.ApplyModify(skill);
                    foreach (var component in skill.skillComponents.ToList())
                        skillComponent.UpdateModify(component);
                    break;
                case SkillExecuteType.MultiplyModifier:
                    skillComponent.ApplyModify(skill);
                    foreach (var component in skill.skillComponents.ToList())
                        skillComponent.UpdateModify(component);
                    break;
            }
        }
        
        DataManager.Inst.Data.skillUpgradeTickets -= skill.upgradeCount + 1;
        DataManager.Inst.Data.skills[_saveIndex] = skill;
        SkillManager.Inst.UpgradeSkill(skill);
        skillUpgradeGroup.UpdateTicketLabel();

        skill.upgradeCount++;
    }

    private Skill GetUpgradeSkill(Skill skill, List<SkillComponent> skillComponents)
    {
        var copySkill = skill.DeepCopy();
        copySkill.skillComponents.AddRange(skillComponents);

        return copySkill;
    }
    
    public void Hide()
    {
        foreach (var card in cards)
            Destroy(card.gameObject);
        
        cards.Clear();
        panel.SetPosition(PanelStates.Hide, true, 0.25f);
    }
}
