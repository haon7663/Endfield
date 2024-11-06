using DG.Tweening;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    [SerializeField] private TMP_Text healthLabel;
    [SerializeField] private TMP_Text barrierLabel;

    public void UpdateUI(int health, int barrier)
    {
        healthLabel.text = health.ToString();

        barrierLabel.DOFade(barrier > 0 ? 1 : 0, 0.05f);
        barrierLabel.text = $"({barrier.ToString()})";
    }
}
