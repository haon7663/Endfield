using System;
using TMPro;
using UnityEngine;

public class MineralWaterController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldCountTxt;

    private void Start()
    {
        ReCountGold();
    }

    public void ReCountGold()
    {
        goldCountTxt.text = DataManager.Inst.Data.gold.ToString() + " ml";
    }


}
