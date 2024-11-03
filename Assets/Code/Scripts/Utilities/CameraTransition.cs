using UnityEngine;
using DG.Tweening;
using UnityEngine.LightTransport;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class CameraTransition : Singleton<CameraTransition>
{
    [SerializeField] private Vector3 originalRotation;
    [SerializeField] private Vector3 targetRotation;

    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float duration;

    [SerializeField] private AnimationCurve animationCurve;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            RotateAndMoveCamera();
        }
    }
    
    public void RotateAndMoveCamera()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(targetRotation, duration * 0.5f));
        sequence.Join(transform.DOMove((originalPosition + targetPosition) * 0.5f, duration * 0.5f));
        sequence.Append(transform.DORotate(originalRotation, duration * 0.5f));
        sequence.Join(transform.DOMove(targetPosition, duration * 0.5f));
        sequence.SetEase(Ease.Linear);
        sequence.Play();
    }
}
