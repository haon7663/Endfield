using SandyCore;
using UnityEngine;
using UnityEngine.UI;

public class SliderEventPublisher : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private string eventName;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        ApplicationManager.Instance.EventService.PublishFloat(eventName, value);
    }
}
