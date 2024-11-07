using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CameraTransition : Singleton<CameraTransition>
{
    [SerializeField] private Vector3 originalRotation;
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private float rotationDuration;

    [SerializeField] private AnimationCurve animationCurve;

    private Vector3 _originalPosition;

    private void Start()
    {
        _originalPosition = transform.position;
        CameraDown();
    }

    public void CameraUp()
    {
        ++DataManager.Inst.Data.stageCount;
        Debug.Log(DataManager.Inst.Data.stageCount);

        var sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(targetRotation, rotationDuration))
            .InsertCallback(rotationDuration * 0.25f, () => Fade.Inst.FadeOut(SceneManager.GetActiveScene()));
    }

    public void CameraDown()
    {
        transform.position = _originalPosition + Vector3.right * GameManager.Inst.startViewPoint;

        transform.rotation = Quaternion.Euler(targetRotation);
        transform.DORotate(originalRotation, rotationDuration);

    }

    public void EntireCameraMove()
    {
        transform.position = new Vector3(-11, transform.position.y, transform.position.z);
        transform.DOMoveX(75, 10f).SetEase(Ease.Linear);
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
