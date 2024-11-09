using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillHoldPanel : MonoBehaviour
{
    public List<SkillCastingViewer> SkillCastingViewers { get; private set; }
    
    [SerializeField] private SkillCastingViewer castingViewerPrefab;
    [SerializeField] private Image panelImage;

    [SerializeField] private Vector2 firstPosition;
    [SerializeField] private float interval;

    private void Awake()
    {
        SkillCastingViewers = new List<SkillCastingViewer>();
    }

    public void AddSkill(Skill skill)
    {
        var castingViewer = Instantiate(castingViewerPrefab, transform);
        castingViewer.Init(skill);
        castingViewer.onFinished += () => RemoveSkill(castingViewer);
        
        SkillCastingViewers.Add(castingViewer);
        UpdateViewers();
    }

    private void UpdateViewers()
    {
        for (var i = 0; i < SkillCastingViewers.Count; i++)
        {
            var castingViewer = SkillCastingViewers[i];
            castingViewer.SetAnchoredPos(firstPosition + Vector2.up * (interval * i), true);
        }
        
        if (SkillCastingViewers.Count > 0)
            Show();
        else
            Hide();
    }

    private void RemoveSkill(SkillCastingViewer castingViewer)
    {
        SkillCastingViewers.Remove(castingViewer);
        Destroy(castingViewer.gameObject);
        UpdateViewers();
    }
    
    private void Show()
    {
        panelImage.DOKill();
        panelImage.DOFade(1, 0.25f);
    }
    private void Hide()
    {
        panelImage.DOKill();
        panelImage.DOFade(0, 0.25f);
    }
    
}
