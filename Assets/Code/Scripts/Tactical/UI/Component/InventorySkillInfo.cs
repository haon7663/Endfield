using System;
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
        nameLabel.text = skill.label;
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
