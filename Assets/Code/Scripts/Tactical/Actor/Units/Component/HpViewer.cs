using UnityEngine;
using DG.Tweening;
using TMPro;

public class HpViewer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI healthTextPrefab;

    private HpBarController hpBarController;
    private Health health;

    private float displayedHp;

    private Transform healthTextInstance;
    public Transform canvas;

    private void Start()
    {
        hpBarController = GameObject.Find("HpBar Controller").GetComponent<HpBarController>();
        health = GetComponent<Health>();
        canvas = GameObject.Find("HpCanvas").GetComponent<Transform>();
        healthTextInstance = Instantiate(healthTextPrefab, canvas).transform;
        healthTextInstance.GetComponent<TextMeshProUGUI>().text = displayedHp.ToString();


        hpBarController.AddHpBar(transform, healthTextInstance);
        displayedHp = health.curHp;

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
            .OnUpdate(() => healthTextInstance.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(displayedHp).ToString());
    }
}