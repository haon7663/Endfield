using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private SkillUpgradeGroup skillUpgradeGroup;
    
    [SerializeField] private Panel panel;
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Transform skillContent;
    [SerializeField] private Transform relicContent;
    [SerializeField] private InventorySkillInfo skillPrefab;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private InventoryRelicInfo relicPrefab;
    [SerializeField] private ClosePanel closePanel;

    [SerializeField] public List<Card> cardPopUp;
    [SerializeField] private Transform inventory;

    [Header("Relic Holders")]
    [SerializeField] private RelicSOHolder upgradeRelicHolder;
    [SerializeField] private RelicSOHolder maxElixirRelicHolder;
    [SerializeField] private RelicSOHolder hpgenRelicHolder;
    [SerializeField] private RelicSOHolder maxHpRelicHolder;
    [SerializeField] private RelicSOHolder goldRelicHolder;

    private bool _isShown;

    private void Update()
    {
        if (_isShown) return;
        
        if (Input.GetKeyDown(KeyCode.P)&&!UIManager.Inst.AlreadyUIOpen())
            Show();
    }

    public void Show()
    {
        UIManager.Inst.UIShow(true);
        AddInventorySkill();
        SetRelicActive();
        closePanel.onClose += Hide;
        panel.SetPosition(PanelStates.Show, true);
        _isShown = true;
    }

    public void Hide()
    {
        UIManager.Inst.UIShow(false);
        panel.SetPosition(PanelStates.Hide, true);
        closePanel.onClose -= Hide;
        _isShown = false;
    }

    public void AddInventorySkill()
    {
        foreach(var inventorySkill in skillContent.GetComponentsInChildren<InventorySkillInfo>())
            Destroy(inventorySkill.gameObject);
        
        foreach (var skill in DataManager.Inst.Data.skills)
        {
            var invSkill = Instantiate(skillPrefab, skillContent);
            var cardInfo = Instantiate(cardPrefab, inventory);
            
            cardInfo.Init(skill);
            cardInfo.gameObject.SetActive(false);
            cardInfo.GetComponent<CanvasGroup>().blocksRaycasts = false;
            
            cardPopUp.Add(cardInfo);
            RectTransform _rect = cardInfo.GetComponent<RectTransform>();
            _rect.pivot = new Vector2(0, 1);
            invSkill.SetInfo(skill,cardInfo,this);
            invSkill.onClick += skillUpgradeGroup.SetSkill;
        }
    }

    private void SetRelicActive()
    {
        upgradeRelicHolder.gameObject.SetActive(ArtifactManager.Inst.skillUpgradeArtifact > 0);
        maxElixirRelicHolder.gameObject.SetActive(ArtifactManager.Inst.maxElixirArtifact > 0);
        hpgenRelicHolder.gameObject.SetActive(ArtifactManager.Inst.hpRegenArtifact > 0);
        maxHpRelicHolder.gameObject.SetActive(ArtifactManager.Inst.maxHpArtifact > 0);
        goldRelicHolder.gameObject.SetActive(ArtifactManager.Inst.goldArtifact > 0);
    }
}
