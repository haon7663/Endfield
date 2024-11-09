using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private SkillUpgradeGroup skillUpgradeGroup;
    
    [SerializeField] private Panel panel;
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Transform skillContent;
    [SerializeField] private Transform relicContent;
    [SerializeField] private InventorySkillInfo skillPrefab;
    [SerializeField] private InventoryRelicInfo relicPrefab;
    [SerializeField] private ClosePanel closePanel;

    [Header("Relic Holders")]
    [SerializeField] private RelicSOHolder upgradeRelicHolder;
    [SerializeField] private RelicSOHolder maxElixirRelicHolder;
    [SerializeField] private RelicSOHolder hpgenRelicHolder;
    [SerializeField] private RelicSOHolder maxHpRelicHolder;
    [SerializeField] private RelicSOHolder goldRelicHolder;

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
        SetRelicActive();
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
        foreach(var inventorySkill in skillContent.GetComponentsInChildren<InventorySkillInfo>())
            Destroy(inventorySkill.gameObject);
        
        foreach (var skill in DataManager.Inst.Data.skills)
        {
            var invSkill = Instantiate(skillPrefab, skillContent);
            invSkill.SetInfo(skill);
            invSkill.onClick += skillUpgradeGroup.SetSkill;
        }
    }

    private void SetRelicActive()
    {
        upgradeRelicHolder.gameObject.SetActive(ArtifactManager.Inst.skillUpgradeArtifact > 0);
        maxElixirRelicHolder.gameObject.SetActive(ArtifactManager.Inst.maxElixirArtifact > 0);
        hpgenRelicHolder.gameObject.SetActive(ArtifactManager.Inst.hpRegenArtifact > 0);
        maxHpRelicHolder.gameObject.SetActive(ArtifactManager.Inst.maxHpArtifact > 0);
        goldRelicHolder.gameObject.SetActive(ArtifactManager.Inst.goldArtifact > 0);
    }
}
