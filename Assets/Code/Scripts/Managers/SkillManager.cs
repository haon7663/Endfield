using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [SerializeField] private SkillPanelController skillPanelController;
    
    private List<Skill> _skillBuffer;
    private Skill[] _skills = new Skill[3];
    
    private Dictionary<Unit, Skill> _previewSkills = new Dictionary<Unit, Skill>();
    
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => DataManager.Inst);
        
        SetupSkillBuffer();
        ArrangeSkills();
        skillPanelController.SetPanels(_skills, _skillBuffer[0]);
        
        foreach (var skill in _skills)
        {
            /*//Debug.Log(skill.skillComponents.Count);
            foreach (var skillComponent in skill.skillComponents)
            {
                Debug.Log(skillComponent.saveName);
                Debug.Log(skillComponent.ExecuteType.ToString());
            }*/
        }
    }

    public Skill GetSkillAtIndex(int index)
    {
        var skill = _skills[index];
        /*//Debug.Log(skill.skillComponents.Count);
        foreach (var skillComponent in skill.skillComponents)
        {
            Debug.Log(skillComponent.saveName);
            Debug.Log(skillComponent.ExecuteType.ToString());
        }*/
        return skill;
    }

    public void ConsumeSkill(int index)
    {
        index = Mathf.Clamp(index, 0, _skills.Length - 1);
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
            { 
                _skills[i] = PopSkill();
                skillPanelController.PopPanel(i, _skillBuffer[0]);
            }
        }
    }
    
    //어떻게 사용될지는 모르겠지만 필요하면 더 추가해서 활용하셈
    private Skill PopSkill() //스킬 사용 
    {
        var skill = _skillBuffer[0];
        _skillBuffer.RemoveAt(0);
        return skill;
    }

    private void SetupSkillBuffer()  //스킬 랜덤 재배치
    {
        _skillBuffer = DataManager.Inst.Data.skills.ToList();
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
        foreach (var previewSkill in _previewSkills)
        {
            previewSkill.Value.Cancel(previewSkill.Key);
            previewSkill.Value.Print(previewSkill.Key);
        }
    }
}
