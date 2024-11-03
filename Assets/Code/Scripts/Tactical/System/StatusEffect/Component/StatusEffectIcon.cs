using UnityEngine;
using UnityEngine.UI;

public class StatusEffectIcon : MonoBehaviour
{
    [SerializeField] private Image statusIcon;
    [SerializeField] private Image statusActiveTimerFill;

    public void SetSprite(Sprite sprite)
    {
        statusIcon.sprite = sprite;
    }

    public void UpdateFill(float duration)
    {
        statusActiveTimerFill.fillAmount = duration;
    }
}