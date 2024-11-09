using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SkillHoldPanel : MonoBehaviour
{
    public List<SkillCastingViewer> SkillCastingViewers { get; private set; }
    
    [SerializeField] private SkillCastingViewer castingViewerPrefab;

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
    }

    private void RemoveSkill(SkillCastingViewer castingViewer)
    {
        SkillCastingViewers.Remove(castingViewer);
        Destroy(castingViewer.gameObject);
        UpdateViewers();
    }
}
