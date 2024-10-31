using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class Skill
{
    public string name;
    public string label;
    public string description;
    public int elixir;
    public float castingTime;
    public List<SkillComponent> skillComponents = new();  // 스킬을 구성하는 컴포넌트 리스트

    public IEnumerator Use(Unit user)
    {
        user.additionalKey = 0;
        foreach (var component in skillComponents) 
        {
            component.Execute(user);  // 각 컴포넌트의 동작 실
            yield return null;
        }
        SkillManager.Inst.RevertSkillArea(user, this);
    }

    public void Print(Unit user)
    {
        user.additionalKey = 0;
        foreach (var component in skillComponents)
        {
            component.Print(user);
        }
    }

    public void Cancel(Unit user)
    {
        foreach (var component in skillComponents)
        {
            component.Cancel(user);
        }
    }
    
    public Skill(string name)
    {
        this.name = name;
    }
}
