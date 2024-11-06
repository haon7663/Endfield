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

    private int _saveIndex;

    public void Show(Skill skill)
    {
        _saveIndex = DataManager.Inst.Data.skills.FindIndex(s => s == skill);
        
        var skillComponents = SkillLoader.GetAllSkillComponent("upgradeSkill");
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
        }
        
        panel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
        closePanel.onClose += Hide;
    }

    private void UpgradeSkill(Skill skill, SkillComponent skillComponent)
    {
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
        
        DataManager.Inst.Data.skills[_saveIndex] = skill;
    }

    private Skill SkillUpgrader(Skill skill, SkillComponent skillComponent)
    {
        var copySkill = skill.DeepCopy();
        copySkill.skillComponents.Add(skillComponent);

        return copySkill;
    }
    
    public void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true, 0.25f);
    }
}
