using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopRelic : ShopItem
{
    [SerializeField] private TextMeshProUGUI itemNameTxt;
    [SerializeField] private Image icon;
    RelicSO _relicSO;
    protected override void Start()
    {
        base.Start();
        RandomRelicInput();
    }

    public void RandomRelicInput()
    {
        var relics = ArtifactManager.Inst.GetAllRelics();
        var relicSO = relics[Random.Range(0, relics.Count)];
        itemNameTxt.text = relicSO.name;
        icon.sprite = relicSO.sprite;
        _relicSO = relicSO;
    }

    protected override void BuyAction()
    {
        Debug.Log("buy");
        _relicSO.Execute();
    }
  
}
