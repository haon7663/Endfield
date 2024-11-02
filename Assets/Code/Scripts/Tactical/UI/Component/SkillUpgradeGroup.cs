using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillUpgradeGroup : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text description;
    
    [SerializeField] private GameObject disableButton;

    public void ShowUpgradePanel()
    {
        Debug.Log("업그레이드 패널");
    }

    public void SetSkill(Skill skill)
    {
        skillIcon.sprite = SkillLoader.GetSkillSprite(skill.name);
        nameLabel.text = skill.name;
        description.text = skill.description;
        
        disableButton.SetActive(false);
    }
}
