using UnityEngine;
using TMPro;
public class HpViewerCreator : MonoBehaviour
{
    [SerializeField] private GameObject healthText;
    [SerializeField] private Transform canvas;

    private void Start()
    {
        Instantiate(healthText, canvas);
    }
}
