using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElixirPanelController : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text label;

    private void LateUpdate()
    {
        fill.fillAmount = GameManager.Inst.curElixir - Mathf.FloorToInt(GameManager.Inst.curElixir);
        label.text = $"{Mathf.FloorToInt(GameManager.Inst.curElixir)}/{GameManager.Inst.maxElixir}";
    }
}
