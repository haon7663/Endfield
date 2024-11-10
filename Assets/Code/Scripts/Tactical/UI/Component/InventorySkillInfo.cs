using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySkillInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerMoveHandler
{
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private Image icon;
    [SerializeField] private Card cardInfo;
    private InventoryController _inventoryController;

    public Action<Skill> onClick;
    private Skill _skill;

    private RectTransform _rectTransform;
    private Vector2 _originSizeDelta;
    private RectTransform _cardRect;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originSizeDelta = _rectTransform.sizeDelta;
    }
    
    public void SetInfo(Skill skill,Card cardInfo ,InventoryController inventoryController)
    {
        this.cardInfo = cardInfo;
        _inventoryController = inventoryController;
        _cardRect = cardInfo.GetComponent<RectTransform>();

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
    
    
    public void OnPointerMove(PointerEventData eventData)
    {
        
        if (_cardRect != null)
        {
            _cardRect.transform.position = Input.mousePosition;
            if (_cardRect.transform.position.y <= 300)
            {
                _cardRect.pivot = new Vector2(0, 0);
            }
            else
            {
                _cardRect.pivot = new Vector2(0, 1);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardInfo)
            foreach (var card in _inventoryController.cardPopUp)
            {
                if (card == cardInfo)
                {
                    card.gameObject.SetActive(true);
                    return;
                }
            }
        
        _rectTransform.DOSizeDelta(_originSizeDelta * 1.05f, 0.25f).SetEase(Ease.OutCirc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardInfo)
            foreach (var card in _inventoryController.cardPopUp)
            {
                if (card == cardInfo)
                {
                    card.gameObject.SetActive(false);
                    return;
                }
            }
        _rectTransform.DOSizeDelta(_originSizeDelta, 0.25f).SetEase(Ease.OutCirc);
    }
}
