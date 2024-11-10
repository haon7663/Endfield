using System;
using DG.Tweening;
using UnityEngine;

public class SkillChangeController : MonoBehaviour
{
    [SerializeField] private Panel panel;
    [SerializeField] private ClosePanel closePanel;
    [SerializeField] private InventorySkillInfo skillPrefab;
    [SerializeField] private Transform inventoryContent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Show();
        }
    }

    public void Show()
    {
        AddInventorySkill();
        panel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
        closePanel.onClose += Hide;
    }
    public void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true, 0.25f);
    }
    
    public void AddInventorySkill()
    {
        foreach (var inventorySkill in inventoryContent.GetComponentsInChildren<InventorySkillInfo>())
            Destroy(inventorySkill.gameObject);

        foreach (var skill in DataManager.Inst.Data.skills)
        {
            var inventory = Instantiate(skillPrefab, inventoryContent);
            inventory.SetInfo(skill);
        }
    }
}
