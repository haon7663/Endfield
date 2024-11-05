using System;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public GameObject content;
    public EventNpc npc;
    public void Show()
    {
        content.transform.position = new Vector3(CameraTransition.Inst.gameObject.transform.position.x,transform.position.y,transform.position.z);
        content.SetActive(true);
    }

    public void Hide()
    {
        content.SetActive(false);
    }

    public void GambleWin(Sprite spr,String iconName)
    {
        npc.ResultSprite(spr,iconName);
    }

    public void GambleFail()
    {
        npc.GambleFail();
    }
}
