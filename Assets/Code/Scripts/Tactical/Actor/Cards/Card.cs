using System;
using System.Linq;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Skill SkillData { get; private set; }
    public Action onClick;
    [SerializeField] private bool isAnim = true;
    
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text elixirLabel;
    [SerializeField] private TMP_Text totalDamageLabel;
    
    private RectTransform _rectTransform;
    private Vector2 _originLocalScale;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originLocalScale = transform.localScale;
    }

    public void ShowAnim()
    {
        DOVirtual.Float(-45f, 0, 1f, value =>
        {
            var axis = _rectTransform.sizeDelta;
            var rotation = Quaternion.AngleAxis(value, axis);
            transform.rotation = rotation;
        }).SetEase(Ease.OutBack);
    }

    public void Init(Skill skillData)
    {
        SkillData = skillData;
        icon.sprite = SkillLoader.GetSkillSprite(skillData.name);
        
        var nameStringBuilder = new StringBuilder();
        nameStringBuilder.Append(skillData.label);
        nameStringBuilder.Append("<color=#FFFF00>");
        foreach (var subName in skillData.skillComponents.Select(s => s.saveName).Where(s => s != skillData.name))
        {
            if (string.IsNullOrEmpty(subName)) continue;
            nameStringBuilder.Append(" ");
            nameStringBuilder.Append(subName);
        }
        nameStringBuilder.Append("</color>");
        nameLabel.text = nameStringBuilder.ToString();

        var descriptionStringBuilder = new StringBuilder();
        descriptionStringBuilder.Append(skillData.description);
        descriptionStringBuilder.Append("<color=#FFFF00>");
        foreach (var subDescription in skillData.skillComponents.Select(s => s.subDescription))
        {
            if (string.IsNullOrEmpty(subDescription)) continue;
            descriptionStringBuilder.Append("\n");
            descriptionStringBuilder.Append(subDescription);
        }
        descriptionStringBuilder.Append("</color>");
        
        description.text = descriptionStringBuilder.ToString();
        elixirLabel.text = skillData.elixir.ToString();

        var totalValue = skillData.skillComponents
            .OfType<AttackComponent>().Where(c => c.ExecuteType == SkillExecuteType.Default)
            .Sum(attackComponent => attackComponent.value);

        totalDamageLabel.gameObject.SetActive(totalValue > 0);
        totalDamageLabel.text = $"기본 피해량: {totalValue}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(_originLocalScale * 1.05f, 0.25f).SetEase(Ease.OutCirc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(_originLocalScale, 0.25f).SetEase(Ease.OutCirc);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAnim)
        {
            onClick?.Invoke();
        }
    }
}