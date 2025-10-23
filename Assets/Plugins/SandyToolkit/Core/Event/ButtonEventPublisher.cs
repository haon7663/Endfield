using SandyCore;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEventPublisher : MonoBehaviour
{
    [SerializeField] private string triggerName;
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(Publish);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(Publish);
    }

    public void Publish()
    {
        ApplicationManager.Instance.EventService.PublishTrigger(triggerName);
    }
}
