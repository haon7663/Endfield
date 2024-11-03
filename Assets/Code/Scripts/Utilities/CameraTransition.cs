using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.LightTransport;
using UnityEngine.Rendering;

public class CameraTransition : Singleton<CameraTransition>
{
    [SerializeField] private Vector3 originalRotation;
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private float rotationDuration;

    [SerializeField] private AnimationCurve animationCurve;


    private void Awake()
    {
        CameraDown();
    }

    public void CameraUp()
    {
        transform.DORotate(targetRotation, rotationDuration)
            .OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void CameraDown()
    {
        transform.rotation = Quaternion.Euler(targetRotation);
        transform.DORotate(originalRotation, rotationDuration);
    }


    /*public void RotateAndMoveCamera()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(targetRotation, rotationDuration));
        sequence.Join(transform.DOMove(targetPosition, 2));
        sequence.Insert(1,transform.DORotate(originalRotation, rotationDuration));
        sequence.SetEase(animationCurve);
        sequence.Play();
    }*/
}
