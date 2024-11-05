using System;
using System.Collections.Generic;
using DG.Tweening;
using GDX.Collections.Generic;
using UnityEngine;

public class SkillPanelController : MonoBehaviour
{
    [SerializeField] private Transform[] skillFrames;
    [SerializeField] private Transform prevSkillFrame;

    [SerializeField] private SkillPanel skillPanelPrefab;
    [SerializeField] private Transform skillPanelParent;

    private SkillPanel[] _skillPanels;
    private SkillPanel _prevSkillPanel;

    private bool _isSet;
    
    private void Awake()
    {
        _skillPanels = new SkillPanel[3];
    }

    public void SetPanels(Skill[] skills, Skill prevSkill)
    {
        _isSet = true;
        
        for (var i = 0; i < 3; i++)
        {
            var skillPanel = Instantiate(skillPanelPrefab, skillFrames[i]);
            skillPanel.Init(skills[i]);
            SetMainPanel(skillPanel, false);
            _skillPanels[i] = skillPanel;
        }
        
        var prevSkillPanel = Instantiate(skillPanelPrefab, prevSkillFrame);
        prevSkillPanel.Init(prevSkill);
        prevSkillPanel.AddSkill();
        _prevSkillPanel = prevSkillPanel;
    }
    
    public void PopPanel(int nullIndex, Skill newSkill)
    {
        if (!_isSet) return;
        
        _skillPanels[nullIndex].RemoveSkill();
        
        _prevSkillPanel.transform.SetParent(skillFrames[nullIndex]);
        SetMainPanel(_prevSkillPanel, true);
        _skillPanels[nullIndex] = _prevSkillPanel;
        
        var prevSkillPanel = Instantiate(skillPanelPrefab, prevSkillFrame);
        prevSkillPanel.Init(newSkill);
        prevSkillPanel.AddSkill();
        _prevSkillPanel = prevSkillPanel;
    }

    private void SetMainPanel(SkillPanel panel, bool useDotween)
    {
        panel.MoveTransform(new PRS(Vector3.zero, Quaternion.identity, new Vector2(71, 71)), useDotween);
    }
}
