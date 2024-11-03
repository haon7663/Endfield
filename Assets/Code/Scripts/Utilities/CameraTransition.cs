using UnityEngine;
using DG.Tweening;
using UnityEngine.LightTransport;
using UnityEngine.Rendering;

public class CameraTransition : Singleton<CameraTransition>
{
    [SerializeField] private Transform _camera;

    [SerializeField] private Vector3 originalRotation;
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private float rotationDuration;

    [SerializeField] private Vector3 targetPosition;

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
        sequence.Append(transform.DORotate(targetRotation, rotationDuration));
        sequence.Join(transform.DOMove(targetPosition, 2));
        sequence.Insert(1,transform.DORotate(originalRotation, rotationDuration));
        sequence.SetEase(animationCurve);
        sequence.Play();
    }
}
