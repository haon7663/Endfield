using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DefeatPanelController : MonoBehaviour
{
    [SerializeField] private Panel panel;
    [SerializeField] private InventorySkillInfo skillPrefab;
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private ClosePanel closePanel;
    [SerializeField] private TMP_Text plantKill;
    [SerializeField] private TMP_Text elitePlantKill;
    [SerializeField] private TMP_Text gainedSkill;
    [SerializeField] private TMP_Text gainedArtifact;

    private void Start()
    {
        Debug.Log(inventoryContent.childCount);
    }

    public void Show()
    {
        AddInventorySkill();
        panel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
        closePanel.onClose += Hide;
        ResetText();  
       
       
    }
    public void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true, 0.25f);
        
        ArtifactManager.Inst.ResetArtifact();
        DataManager.Inst.Generate("Player");
        DataManager.Inst.ResetData();
        SceneManager.LoadScene("Title");
    }

    private void ResetText()
    {
        gainedArtifact.text = DataManager.Inst.Data.artifactCount.ToString();
        gainedSkill.text = DataManager.Inst.Data.skillCount.ToString();
        elitePlantKill.text = DataManager.Inst.Data.eliteKillCount.ToString();
        plantKill.text =  DataManager.Inst.Data.plantKillCount.ToString();
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
