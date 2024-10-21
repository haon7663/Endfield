using UnityEngine;

public class HpBarController : MonoBehaviour
{
    public Camera mainCamera;
    public Transform targetObject;

    void Update()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetObject.position);
    }
}
