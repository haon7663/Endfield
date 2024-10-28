using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextHud : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(string text)
    {
        label.text = text;

        var sequence = DOTween.Sequence();
        sequence.Append(_rectTransform.DOAnchorPosY(100, 0.5f).SetRelative())
            .Append(label.DOFade(0, 0.1f));
    }
}
