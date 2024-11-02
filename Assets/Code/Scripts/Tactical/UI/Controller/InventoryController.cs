using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private SkillUpgradeGroup skillUpgradeGroup;
    
    [SerializeField] private Panel panel;
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Transform inventoryContent, relicContent;
    [SerializeField] private InventorySkillInfo skillPrefab;
    [SerializeField] private GameObject relicPrefab;
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
        
        foreach (var skill in DataManager.Inst.Data.skills)
        {
            var inventory = Instantiate(skillPrefab, inventoryContent);
            inventory.SetInfo(skill);
            inventory.onClick += skillUpgradeGroup.SetSkill;
        }
    }

    public void AddRelic()
    {
        Instantiate(relicPrefab, relicContent);
    }
}
