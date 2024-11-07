using UnityEngine;
using UnityEngine.EventSystems;

public class RelicSOHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RelicSO relicSO;
    [SerializeField] private InventoryRelicInfo relicInfo;
    [SerializeField] private GameObject relicUIPrefab;
    private GameObject instantiatedRelicUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        relicInfo.InitRelicInfo(relicSO, AmountCalcul());

        instantiatedRelicUI = Instantiate(relicUIPrefab, transform);
        RectTransform uiRectTransform = instantiatedRelicUI.GetComponent<RectTransform>();

        if (uiRectTransform != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 offsetPosition = new Vector3(0, uiRectTransform.rect.height / 2, 0);

            uiRectTransform.position = mousePosition + offsetPosition;
            uiRectTransform.pivot = new Vector2(0, 0); 
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (instantiatedRelicUI != null)
        {
            Destroy(instantiatedRelicUI);
        }
    }

    private int AmountCalcul()
    {
        return relicSO.relicType switch
        {
            RelicSO.RelicType.HpRegen => ArtifactManager.Inst.hpRegenArtifact,
            RelicSO.RelicType.GetGold => ArtifactManager.Inst.goldArtifact,
            RelicSO.RelicType.MaxElixier => ArtifactManager.Inst.maxElixirArtifact,
            RelicSO.RelicType.GetSkillUpgrade => ArtifactManager.Inst.skillUpgradeArtifact,
            RelicSO.RelicType.MaxHp => ArtifactManager.Inst.maxHpArtifact,
            _ => 0 
        };
    }
}
