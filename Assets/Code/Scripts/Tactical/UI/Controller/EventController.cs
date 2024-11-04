using UnityEngine;

public class EventController : MonoBehaviour
{
    public GameObject content;
    public EventNpc npc;
    public void Show()
    {
        content.SetActive(true);
    }

    public void Hide()
    {
        content.SetActive(false);
    }

    public void GambleResult(Sprite spr)
    {
        npc.ResultSprite(spr);
    }
}
