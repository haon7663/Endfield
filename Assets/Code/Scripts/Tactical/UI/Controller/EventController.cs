using UnityEngine;

public class EventController : MonoBehaviour
{
    public GameObject content;

    public void Show()
    {
        content.SetActive(true);
    }

    public void Hide()
    {
        content.SetActive(false);
    }
}
