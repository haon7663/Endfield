using System;
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
    public List<SkillComponent> SkillComponents = new();  // 스킬을 구성하는 컴포넌트 리스트

    public void Use(Unit user)
    {
        foreach (var component in SkillComponents) 
        {
            component.Execute(user);  // 각 컴포넌트의 동작 실행
        }
    }

    public void Print(Unit user)
    {
        foreach (var component in SkillComponents)
        {
            component.Print(user);
        }
    }
    
    public Skill(string name)
    {
        this.name = name;
    }
}
