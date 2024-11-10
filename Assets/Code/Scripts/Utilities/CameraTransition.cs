using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CameraTransition : Singleton<CameraTransition>
{
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 originalRotation;
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private float duration;

    [SerializeField] private AnimationCurve animationCurve;

    private void Start()
    {
        CameraDown();
    }

    public void CameraUp()
    {
        ++DataManager.Inst.Data.stageCount;

        var sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(targetRotation, duration))
            //.Join(transform.DOMove(targetPosition + Vector3.right * GameManager.Inst.startViewPoint, duration))
            .InsertCallback(duration * 0.25f, () => Fade.Inst.FadeOut(SceneManager.GetActiveScene()));
    }

    public void CameraDown()
    {
        transform.position = originalPosition + Vector3.right * GameManager.Inst.startViewPoint;
        
        transform.rotation = Quaternion.Euler(targetRotation);
        transform.DORotate(originalRotation, duration);
        //transform.DOMove(originalPosition + Vector3.right * GameManager.Inst.startViewPoint, duration);
    }

    public void EntireCameraMove()
    {
        transform.position = new Vector3(-11, transform.position.y, transform.position.z);
        transform.DOMoveX(75, 10f).SetEase(Ease.Linear);
    }
}
