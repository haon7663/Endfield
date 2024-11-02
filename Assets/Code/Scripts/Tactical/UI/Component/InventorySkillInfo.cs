using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySkillInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private Image icon;

    public void SetInfo(Skill skill)
    {
        nameLabel.text = skill.name;
        icon.sprite = SkillLoader.GetSkillSprite(skill.name);
    }
}
