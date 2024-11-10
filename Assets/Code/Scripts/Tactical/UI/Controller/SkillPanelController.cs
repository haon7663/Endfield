using System;
using System.Collections.Generic;
using DG.Tweening;
using GDX.Collections.Generic;
using UnityEngine;

public class SkillPanelController : MonoBehaviour
{
    [SerializeField] private SkillFrame[] skillFrames;
    [SerializeField] private SkillFrame prevSkillFrame;

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
            var skillPanel = Instantiate(skillPanelPrefab, skillFrames[i].transform);
            skillPanel.Init(skills[i]);
            skillFrames[i].Init(skills[i]);
            SetMainPanel(skillPanel, false);
            _skillPanels[i] = skillPanel;
        }
        
        var prevSkillPanel = Instantiate(skillPanelPrefab, prevSkillFrame.transform);
        prevSkillPanel.Init(prevSkill);
        prevSkillFrame.Init(prevSkill);
        prevSkillPanel.AddSkill();
        _prevSkillPanel = prevSkillPanel;
    }
    
    public void UpdatePanel(int index, Skill updatedSkill)
    {
        // 특정 패널을 업데이트합니다.
        skillFrames[index].Init(updatedSkill);
        _skillPanels[index].Init(updatedSkill);
    }
    
    public void PopPanel(int nullIndex, Skill newSkill)
    {
        if (!_isSet) return;
        
        _skillPanels[nullIndex].RemoveSkill();
        
        _prevSkillPanel.transform.SetParent(skillFrames[nullIndex].transform);
        skillFrames[nullIndex].Init(_prevSkillPanel.Data);
        SetMainPanel(_prevSkillPanel, true);
        _skillPanels[nullIndex] = _prevSkillPanel;
        
        var prevSkillPanel = Instantiate(skillPanelPrefab, prevSkillFrame.transform);
        prevSkillPanel.Init(newSkill);
        prevSkillFrame.Init(newSkill);
        prevSkillPanel.AddSkill();
        _prevSkillPanel = prevSkillPanel;
    }

    private void SetMainPanel(SkillPanel panel, bool useDotween)
    {
        panel.MoveTransform(new PRS(Vector3.zero, Quaternion.identity, new Vector2(71, 71)), useDotween);
    }
}
