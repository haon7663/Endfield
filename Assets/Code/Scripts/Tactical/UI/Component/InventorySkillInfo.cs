using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySkillInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private Image icon;

    public Action<Skill> onClick;
    private Skill _skill;

    public void SetInfo(Skill skill)
    {
        nameLabel.text = skill.name;
        icon.sprite = SkillLoader.GetSkillSprite(skill.name);

        _skill = skill;
    }

    public void OnClick()
    {
        onClick?.Invoke(_skill);
    }
}
