using UnityEngine;
using System.Collections.Generic;

public class HpBarController : MonoBehaviour
{
    public Camera mainCamera;

    public List<Transform> targetObjects = new List<Transform>();
    public List<Transform> healthTexts = new List<Transform>();

    private void Update()
    {
        for (int i = 0; i < targetObjects.Count; i++)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetObjects[i].position);

            healthTexts[i].position = screenPosition;
        }
    }

    public void AddHpBar(Transform targetObject, Transform healthText)
    {
        targetObjects.Add(targetObject);
        healthTexts.Add(healthText);
    }
}