using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image backGround;

    public void OnPointerEnter(PointerEventData eventData)
    {
        backGround.transform.DOLocalRotateQuaternion(Quaternion.identity, 0.5f).SetEase(Ease.OutCirc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backGround.transform.DOLocalRotateQuaternion(Quaternion.Euler(24, 24, 8f), 0.5f).SetEase(Ease.OutCirc);
    }
    
    public float rotationSpeed = 30f; // 회전 속도

    // void Update()
    // {
    //     var point1 = transform.position 
    //     // 빨간 선의 방향 벡터 계산
    //     Vector3 axis = (point2.position - point1.position).normalized;
    //
    //     // 선의 중간 지점을 회전 중심으로 설정
    //     Vector3 center = (point1.position + point2.position) / 2;
    //
    //     // 해당 축과 중심을 기준으로 회전
    //     transform.RotateAround(center, axis, rotationSpeed * Time.deltaTime);
    // }
}