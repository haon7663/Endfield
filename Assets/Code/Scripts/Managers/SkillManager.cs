using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [SerializeField] private SkillPanelController skillPanelController;
    
    private List<Skill> _skillBuffer;
    private Skill[] _skills = new Skill[4];
    
    private void Start()
    {
        SetupSkillBuffer();
        ArrangeSkills();
    }

    public Skill GetSkillAtIndex(int index)
    {
        index = Mathf.Clamp(index, 0, 3);
        var skill = _skills[index];
        _skillBuffer.Add(skill);
        
        _skills[index] = null;
        ArrangeSkills();
        
        return skill;
    }
    
    public void ArrangeSkills()
    {
        for (var i = 0; i < _skills.Length; i++)
        {
            if (_skills[i] == null || string.IsNullOrEmpty(_skills[i].name))
                _skills[i] = PopSkill();
        }
        skillPanelController.UpdatePanels(_skills, _skillBuffer[0]);
    }
    
    //어떻게 사용될지는 모르겠지만 필요하면 더 추가해서 활용하셈
    private Skill PopSkill() //스킬 사용 
    {
        if (_skillBuffer.Count == 0) SetupSkillBuffer();

        var skill = _skillBuffer[0];
        _skillBuffer.RemoveAt(0);
        return skill;
    }

    private void SetupSkillBuffer()  //스킬 랜덤 재배치
    {
        _skillBuffer = SkillLoader.GetSkills("skill");
        _skillBuffer.Shuffle();
    }
}
