using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static ArtifactManager;

public class RelicSOHolder : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
{
    [SerializeField] private RelicSO relicSO;
    [SerializeField] private InventoryRelicInfo relicInfo;
    
    private RectTransform _relicInfoRect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        relicInfo.gameObject.SetActive(true);
        
        relicInfo.Init(relicSO, ArtifactCalculate());
        _relicInfoRect = relicInfo.GetComponent<RectTransform>();
    }
    
    public void OnPointerMove(PointerEventData eventData)
    {
        if (_relicInfoRect != null)
        {
            _relicInfoRect.transform.position = Input.mousePosition;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_relicInfoRect != null)
        {
            relicInfo.gameObject.SetActive(false);
        }
    }

    private int ArtifactCalculate()
    {
        return relicSO.relicType switch
        {
            RelicSO.RelicType.HpRegen => ArtifactManager.Inst.CalculateArtifactValue(ArtifactType.HpRegen),
            RelicSO.RelicType.GetGold => ArtifactManager.Inst.CalculateArtifactValue(ArtifactType.Gold),
            RelicSO.RelicType.MaxElixier => ArtifactManager.Inst.CalculateArtifactValue(ArtifactType.MaxElixir),
            RelicSO.RelicType.GetSkillUpgrade => ArtifactManager.Inst.CalculateArtifactValue(ArtifactType.SkillUpgradeTicket),
            RelicSO.RelicType.MaxHp => ArtifactManager.Inst.CalculateArtifactValue(ArtifactType.MaxHp),
            _ => 0 
        };
    }
}
