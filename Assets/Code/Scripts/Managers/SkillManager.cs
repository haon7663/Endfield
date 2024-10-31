using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [SerializeField] private SkillPanelController skillPanelController;
    
    private List<Skill> _skillBuffer;
    private Skill[] _skills = new Skill[4];
    
    private Dictionary<Unit, Skill> _previewSkills = new Dictionary<Unit, Skill>();
    
    private void Start()
    {
        SetupSkillBuffer();
        ArrangeSkills();
    }

    public Skill GetSkillAtIndex(int index)
    {
        var skill = _skills[index];
        return skill;
    }

    public void ConsumeSkill(int index)
    {
        index = Mathf.Clamp(index, 0, 3);
        var skill = _skills[index];
        _skillBuffer.Add(skill);
        
        _skills[index] = null;
        ArrangeSkills();
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
        _skillBuffer = SkillLoader.GetAllSkills("skill");
        _skillBuffer.Shuffle();
    }
    
    public void ApplySkillArea(Unit user, Skill skill)
    {
        _previewSkills[user] = skill;
        skill.Print(user);
    }
    public void RevertSkillArea(Unit user)
    {
        if (_previewSkills.ContainsKey(user))
            _previewSkills.Remove(user);
    }
    public void UpdateSkillArea()
    {
        Debug.Log("UpdateSkillArea");
        foreach (var previewSkill in _previewSkills)
        {
            previewSkill.Value.Cancel(previewSkill.Key);
            previewSkill.Value.Print(previewSkill.Key);
        }
    }
}
