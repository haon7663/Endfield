using System;
using System.Linq;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySkillInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private Image icon;

    public Action<Skill> onClick;
    private Skill _skill;

    private RectTransform _rectTransform;
    private Vector2 _originSizeDelta;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originSizeDelta = _rectTransform.sizeDelta;
    }

    public void SetInfo(Skill skill)
    {
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

        icon.sprite = SkillLoader.GetSkillSprite(skill.name);

        _skill = skill;
    }

    public void OnClick()
    {
        onClick?.Invoke(_skill);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _rectTransform.DOSizeDelta(_originSizeDelta * 1.05f, 0.25f).SetEase(Ease.OutCirc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rectTransform.DOSizeDelta(_originSizeDelta, 0.25f).SetEase(Ease.OutCirc);
    }
}
