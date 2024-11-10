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

        /*var saveName = "";
        foreach (var skillComponent in skillComponents)
        {
            var componentName = skillComponent.saveName;
            if (string.IsNullOrEmpty(skillComponent.saveName))
                componentName = saveName;
            
            saveName = skillComponent.saveName;

            if (skill != null)
            {
                skill.skillComponents.Add(skillComponent);
            }
            else
            {
                var newSkill = skillData.FirstOrDefault(s => s.name == skillComponent.saveName);
                if (newSkill == null) continue;
                newSkill.skillComponents.Add(skillComponent);
                skills.Add(newSkill);
            }
        }*/
        
        var haveComponents = new List<SkillComponent>();
        for (var i = 0; i < 3; i++)
        {
            var skillComponent = skillComponents.Where(s => !haveComponents.Contains(s)).ToList().Random();
            haveComponents.Add(skillComponent);
            
            var newSkill = SkillUpgrader(skill, skillComponent);
            
            var card = Instantiate(cardPrefab, cardGroup);
            card.Init(newSkill);
            card.onClick += Hide;
            card.onClick += () => UpgradeSkill(newSkill, skillComponent);
            
            cards.Add(card);
        }
        
        panel.SetPosition(PanelStates.Show, true, 0.3f, Ease.OutBack);
        closePanel.onClose += Hide;
    }

    private void UpgradeSkill(Skill skill, SkillComponent skillComponent)
    {
        if (DataManager.Inst.Data.skillUpgradeTickets < skill.upgradeCount + 1) return;
        
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
        
        Debug.Log(skill.upgradeCount);
        
        DataManager.Inst.Data.skillUpgradeTickets -= skill.upgradeCount + 1;
        DataManager.Inst.Data.skills[_saveIndex] = skill;
        skillUpgradeGroup.UpdateTicketLabel();

        skill.upgradeCount++;
    }

    private Skill SkillUpgrader(Skill skill, SkillComponent skillComponent)
    {
        var copySkill = skill.DeepCopy();
        copySkill.skillComponents.Add(skillComponent);

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
