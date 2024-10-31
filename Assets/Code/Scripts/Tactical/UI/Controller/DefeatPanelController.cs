using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using TMPro;
using System.Collections.Generic;

public class DefeatPanelController : MonoBehaviour
{
    [SerializeField] private Panel panel;
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private ClosePanel closePanel;
    [SerializeField] private TMP_Text plantKill;
    [SerializeField] private TMP_Text elitePlantKill;
    [SerializeField] private TMP_Text gainedSkill;
    [SerializeField] private TMP_Text gainedArtifact;
    [SerializeField] private InventoryController inventoryController;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Show();
        }
    }

    private void Show()
    {
        AddInventorySkill();
        panel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
        closePanel.onClose += Hide;
        ResetText();
    }
    public void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true, 0.25f);
    }

    private void ResetText()
    {
        gainedArtifact.text = DataManager.Inst.Data.gainedArtifactCount.ToString();
        gainedSkill.text = DataManager.Inst.Data.gainedSkillCount.ToString();
        elitePlantKill.text = DataManager.Inst.Data.eliteKillCount.ToString();
        plantKill.text =  DataManager.Inst.Data.plantKillCount.ToString();
    }

    public void AddInventorySkill()
    {
        foreach (var inventorySkill in inventoryContent.GetComponentsInChildren<InventorySkillInfo>())
            Destroy(inventorySkill.gameObject);

        List<Skill> skills = DataManager.Inst.Data.skills;
        foreach (var skill in skills)
        {
            GameObject inventory = Instantiate(skillPrefab, inventoryContent);
            if (inventory.TryGetComponent(out InventorySkillInfo inventorySkillInfo))
                inventorySkillInfo.SetInfo(skill);
        }
    }


}
