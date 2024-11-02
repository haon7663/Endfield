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
    
    [SerializeField] private GameObject disableButton;

    private Skill _skill;

    public void ShowUpgradePanel()
    {
        Debug.Log("업그레이드 패널");
        skillUpgradePanel.Show(_skill);
        inventoryController.Hide();
    }

    public void SetSkill(Skill skill)
    {
        skillIcon.sprite = SkillLoader.GetSkillSprite(skill.name);
        nameLabel.text = skill.name;
        description.text = skill.description;
        
        disableButton.SetActive(false);

        _skill = skill;
    }
}
