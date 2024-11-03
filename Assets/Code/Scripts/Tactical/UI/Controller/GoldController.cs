using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GoldController : Singleton<GoldController>
{
    [SerializeField] private TextMeshProUGUI goldCountTxt;

    private void Start()
    {
        ReCountGold(0,DataManager.Inst.Data.gold);
    }

    public void ReCountGold(int startValue,int endValue)
    {
        goldCountTxt.text = startValue.ToString();
        DOTween.To(() => startValue, x =>
        {
            startValue = x;
            goldCountTxt.text = startValue.ToString()+ " ml";
        }, endValue, 0.8f);
    }


}
