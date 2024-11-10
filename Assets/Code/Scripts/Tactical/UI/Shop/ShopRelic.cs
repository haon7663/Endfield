using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopRelic : ShopItem ,IPointerEnterHandler, IPointerExitHandler,IPointerMoveHandler
{
    [SerializeField] private InventoryRelicInfo inventoryRelicInfo;
    [SerializeField] private TextMeshProUGUI itemNameTxt;
    [SerializeField] private Image icon;
    RelicSO _relicSO;

    private RectTransform _relicInfoRect;
    protected override void Start()
    {
        inventoryRelicInfo.gameObject.SetActive(false);
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
        _relicInfoRect = inventoryRelicInfo.GetComponent<RectTransform>();
        inventoryRelicInfo.Init(_relicSO,ArtifactCalculate(_relicSO));
    }

    protected override void BuyAction()
    {
        Debug.Log("buy");
        _relicSO.Execute();
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        if(_relicInfoRect)
            _relicInfoRect.transform.position = Input.mousePosition;
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryRelicInfo.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryRelicInfo.gameObject.SetActive(false);
    }
    
    private int ArtifactCalculate(RelicSO relicSO)
    {
        return relicSO.relicType switch
        {
            RelicSO.RelicType.HpRegen => ArtifactManager.Inst.CalculateArtifactValue(ArtifactManager.ArtifactType.HpRegen,1),
            RelicSO.RelicType.GetGold => ArtifactManager.Inst.CalculateArtifactValue(ArtifactManager.ArtifactType.Gold,1),
            RelicSO.RelicType.MaxElixier => ArtifactManager.Inst.CalculateArtifactValue(ArtifactManager.ArtifactType.MaxElixir,1),
            RelicSO.RelicType.GetSkillUpgrade => ArtifactManager.Inst.CalculateArtifactValue(ArtifactManager.ArtifactType.SkillUpgradeTicket,1),
            RelicSO.RelicType.MaxHp => ArtifactManager.Inst.CalculateArtifactValue(ArtifactManager.ArtifactType.MaxHp,1),
            _ => 0 
        };
    }

  
}

 

