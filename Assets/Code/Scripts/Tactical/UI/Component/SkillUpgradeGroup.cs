using System;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillUpgradeGroup : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private SkillUpgradePanel skillUpgradePanel;
    
    [SerializeField] private Image skillIcon;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text remainUpgradeTicketLabel;
    
    [SerializeField] private Button upgradeButton;
    [SerializeField] private GameObject disableButton;
    
    private Skill _skill;
    private DataManager _dataManagerRef;

    private void Start()
    {
        _dataManagerRef = DataManager.Inst;
    }

    private void Update()
    {
        UpdateTicketLabel();
    }

    public void UpdateTicketLabel()
    {
        remainUpgradeTicketLabel.text = $"남은 강화권 ({_dataManagerRef.Data.skillUpgradeTickets})";
    }

    public void ShowUpgradePanel()
    {
        skillUpgradePanel.Show(_skill);
        inventoryController.Hide();
    }

    public void SetSkill(Skill skill)
    {
        skillIcon.sprite = SkillLoader.GetSkillSprite(skill.name);
        
        var nameStringBuilder = new StringBuilder();
        nameStringBuilder.Append(skill.label);
        nameStringBuilder.Append("<color=#FFFF00>");
        foreach (var subName in skill.skillComponents.Select(s => s.saveName).Where(s => s != skill.name))
        {
            if (string.IsNullOrEmpty(subName)) continue;
            nameStringBuilder.Append(" ");
            nameStringBuilder.Append(subName);
        }
        nameStringBuilder.Append("</color>");
        nameLabel.text = nameStringBuilder.ToString();
        
        var descriptionStringBuilder = new StringBuilder();
        descriptionStringBuilder.Append(skill.description);
        descriptionStringBuilder.Append("<color=#FFFF00>");
        foreach (var subDescription in skill.skillComponents.Select(s => s.subDescription))
        {
            if (string.IsNullOrEmpty(subDescription)) continue;
            descriptionStringBuilder.Append("\n");
            descriptionStringBuilder.Append(subDescription);
        }
        descriptionStringBuilder.Append("</color>");

        description.text = descriptionStringBuilder.ToString();

        upgradeButton.interactable = DataManager.Inst.Data.skillUpgradeTickets >= skill.upgradeCount + 1;
        disableButton.SetActive(DataManager.Inst.Data.skillUpgradeTickets < skill.upgradeCount + 1);
        _skill = skill;
    }
}
