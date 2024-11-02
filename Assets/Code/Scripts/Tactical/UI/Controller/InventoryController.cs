using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Panel panel;
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Transform inventoryContent, relicContent;
    [SerializeField] private GameObject skillPrefab, relicPrefab;
    [SerializeField] private ClosePanel closePanel;

    private bool _isShown;

    private void Update()
    {
        if (_isShown) return;
        
        if (Input.GetKeyDown(KeyCode.P))
            Show();
    }

    public void Show()
    {
        AddInventorySkill();
        closePanel.onClose += Hide;
        panel.SetPosition(PanelStates.Show, true);
        _isShown = true;
    }

    public void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true);
        closePanel.onClose -= Hide;
        _isShown = false;
    }

    public void AddInventorySkill()
    {
        foreach(var inventorySkill in inventoryContent.GetComponentsInChildren<InventorySkillInfo>())
            Destroy(inventorySkill.gameObject);
        
        List<Skill> skills = SkillLoader.GetAllSkills("skill");//DataManager.Inst.Data.skills;
        Debug.Log(skills.Count);
        foreach (var skill in skills)
        {
            GameObject inventory = Instantiate(skillPrefab, inventoryContent);
            if(inventory.TryGetComponent(out InventorySkillInfo inventorySkillInfo))
                inventorySkillInfo.SetInfo(skill);
        }
    }

    public void AddRelic()
    {
        Instantiate(relicPrefab, relicContent);
    }
}
