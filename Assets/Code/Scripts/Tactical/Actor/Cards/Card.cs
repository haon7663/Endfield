using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Skill SkillData { get; private set; }
    public Action onClick;
    
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text elixirLabel;

    private RectTransform _rectTransform;
    private float _rotateValue;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(Skill skillData)
    {
        SkillData = skillData;
        icon.sprite = Resources.Load<Sprite>("Card_Icon/" + skillData.name);
        nameLabel.text = skillData.label;
        description.text = skillData.description;
        elixirLabel.text = skillData.elixir.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DOVirtual.Float(_rotateValue, 0, 0.5f, value =>
        {
            _rotateValue = value;
            var axis = _rectTransform.sizeDelta;
            var rotation = Quaternion.AngleAxis(value, axis);
            transform.rotation = rotation;
        }).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DOVirtual.Float(_rotateValue, -25f, 0.5f, value =>
        {
            _rotateValue = value;
            var axis = _rectTransform.sizeDelta;
            var rotation = Quaternion.AngleAxis(value, axis);
            transform.rotation = rotation;
        }).SetEase(Ease.OutBack);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}