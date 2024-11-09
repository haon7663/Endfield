using System;
using System.Collections;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCastingViewer : MonoBehaviour
{
    public Skill Data { get; private set; }

    public Action onFinished;
    
    [SerializeField] private Image selectSkill;
    [SerializeField] private Image backGround;
    [SerializeField] private Image fill;
    [SerializeField] private Color targetColor;

    [HideInInspector] public Vector2 originPosition;
    
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(Skill skill)
    {
        Data = skill;
        selectSkill.sprite = SkillLoader.GetSkillSprite(skill.spriteName);
    }

    public void SetAnchoredPos(Vector2 anchoredPos, bool useDotween = false, float time = 0.2f)
    {
        if (useDotween)
        {
            _rectTransform.DOAnchorPos(anchoredPos, time);
        }
        else
        {
            _rectTransform.anchoredPosition = anchoredPos;
        }
    }
    
    public IEnumerator Cast()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(DOVirtual.Float(1, 0, Data.castingTime, value => fill.fillAmount = 1 - value)
            .SetEase(Ease.Linear)).Join(backGround.DOColor(targetColor, Data.castingTime));
        
        yield return sequence.WaitForCompletion();
    }

    public void Remove()
    {
        _canvasGroup.DOFade(0, 0.2f).OnComplete(() => onFinished?.Invoke());
    }
}
