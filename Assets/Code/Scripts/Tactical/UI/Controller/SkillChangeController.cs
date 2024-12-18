using System;
using DG.Tweening;
using UnityEngine;

public class SkillChangeController : Singleton<SkillChangeController>
{
    [SerializeField] private Panel panel;
    [SerializeField] private ClosePanel closePanel;
    [SerializeField] private InventorySkillInfo skillPrefab;
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private Card changeCard;

    private Skill _newSkill;
    private Skill _selectedSkill;

    public void Show(Skill newSkill)
    {
        UIManager.Inst.UIShow(true);
        AddInventorySkill();
        panel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
        closePanel.onClose += Hide;

        _newSkill = newSkill;
        changeCard.Init(newSkill);
        changeCard.ShowAnim();
        
        closePanel.gameObject.SetActive(false);
    }
    public void Hide()
    {
        UIManager.Inst.UIShow(false);
        SkillManager.Inst.ChangeSkill(_selectedSkill, _newSkill);
        
        var skills = DataManager.Inst.Data.skills;
        for (var i = 0; i < skills.Count; i++)
            if (skills[i].Id == _selectedSkill.Id)
                skills[i] = _newSkill;
        
        panel.SetPosition(PanelStates.Hide, true, 0.25f);
        
        GridManager.Inst.GenerateTransitionTiles();
    }
    
    public void AddInventorySkill()
    {
        foreach (var inventorySkill in inventoryContent.GetComponentsInChildren<InventorySkillInfo>())
            Destroy(inventorySkill.gameObject);

        foreach (var skill in DataManager.Inst.Data.skills)
        {
            var inventory = Instantiate(skillPrefab, inventoryContent);
            inventory.SetInfo(skill);
            inventory.onClick += s =>
            {
                _selectedSkill = s;
                closePanel.gameObject.SetActive(true);
            };
        }
    }
}
