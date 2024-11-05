using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    [SerializeField] private Image icon;
    
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(Skill skill)
    {
        icon.sprite = SkillLoader.GetSkillSprite(skill.name);
    }

    public void MoveTransform(PRS prs, bool useDotween = false, float duration = 0.2f)
    {
        if (useDotween)
        {
            _rectTransform.DOAnchorPos(prs.Pos, duration);
            _rectTransform.DORotateQuaternion(prs.Rot, duration);
            _rectTransform.DOSizeDelta(prs.Scale, duration);
        }
        else
        {
            _rectTransform.anchoredPosition = prs.Pos;
            _rectTransform.rotation = prs.Rot;
            _rectTransform.sizeDelta = prs.Scale;
        }
    }
    
    public void AddSkill()
    {
        _rectTransform.sizeDelta = new Vector2(50, 50);
        _rectTransform.DOAnchorPos(Vector2.zero, 0.2f).From(new Vector2(0, 100));
        _canvasGroup.DOFade(1, 0.2f).From(0);
    }

    public void RemoveSkill()
    {
        _rectTransform.DOAnchorPos(new Vector2(0, 100), 0.2f);
        _canvasGroup.DOFade(0, 0.2f)
            .OnComplete(() => Destroy(gameObject));
    }
}
