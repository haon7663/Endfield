using UnityEngine;
using DG.Tweening;
using TMPro;

public class HpViewer : MonoBehaviour
{
    private Health health;
    [SerializeField] private TextMeshProUGUI healthText;

    private float displayedHp;

    private void Start()
    {
        health = GetComponent<Health>();
        displayedHp = health.curHp;
        healthText.text = displayedHp.ToString();
    }

    private void Update()
    {
        if (Mathf.RoundToInt(displayedHp) != health.curHp)
        {
            UpdateHealthUI(health.curHp);
        }
    }

    private void UpdateHealthUI(int newHp)
    {
        DOTween.To(() => displayedHp, x => displayedHp = x, newHp, 0.5f)
            .OnUpdate(() => healthText.text = Mathf.RoundToInt(displayedHp).ToString());
    }
}
