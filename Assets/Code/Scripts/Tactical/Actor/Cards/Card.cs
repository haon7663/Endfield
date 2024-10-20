using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image backGround;

    private float _rotateValue;

    public void OnPointerEnter(PointerEventData eventData)
    {
        DOVirtual.Float(_rotateValue, 0, 0.5f, value => _rotateValue = value).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DOVirtual.Float(_rotateValue, -25f, 0.5f, value => _rotateValue = value).SetEase(Ease.OutBack);
    }

    private void Update()
    {
        var axis = rectTransform.sizeDelta;
        var rotation = Quaternion.AngleAxis(_rotateValue, axis);
        transform.rotation = rotation;
    }
}