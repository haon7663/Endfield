using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GoldController : Singleton<GoldController>
{
    [SerializeField] private TextMeshProUGUI goldCountTxt;

    private void Start()
    {
        ReCountGold(0);
    }

    public void ReCountGold(int value)
    {
        int startValue = DataManager.Inst.Data.gold;
        goldCountTxt.text = startValue.ToString();
        DOTween.To(() => startValue, x =>
        {
            startValue = x;
            goldCountTxt.text = startValue.ToString()+ " ml";
        }, startValue + value, 0.8f);
        DataManager.Inst.Data.gold+=value;
    }


}
