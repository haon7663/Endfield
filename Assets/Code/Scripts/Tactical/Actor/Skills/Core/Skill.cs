using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Skill
{
    public string Id;
    
    public string name;
    public string spriteName;
    public string label;
    public string description;
    public int elixir;
    public float castingTime;
    public int executeCount;
    public List<SkillComponent> skillComponents = new();  // 스킬을 구성하는 컴포넌트 리스트
    public int isAnimation;
    public string baseComponent;

    public int upgradeCount;

    public void Setup()
    {
        Id = Guid.NewGuid().ToString();
    }

    public void Use(Unit user)
    {
        user.additionalKey = 0;
        SkillComponent defaultComponent = null;
        var skillComponentInfo = new SkillComponentInfo(user, user.Tile, user.Movement.DirX);
        
        foreach (var component in skillComponents) 
        {
            switch (component.ExecuteType)
            {
                case SkillExecuteType.Default:
                    defaultComponent = component;
                    component.Init(skillComponentInfo);
                    break;
                case SkillExecuteType.OnHit:
                    defaultComponent?.AddOnHit(component.Execute);
                    break;
                case SkillExecuteType.OnEnd:
                    defaultComponent?.AddOnEnd(component.Execute);
                    break;
            }
        }

        foreach (var component in skillComponents.Where(s => s.ExecuteType == SkillExecuteType.Default))
        {
            component.Execute(skillComponentInfo);
        }
        
        SkillManager.Inst.RevertSkillArea(user);
        Cancel(user);
    }

    public void Print(Unit user)
    {
        user.additionalKey = 0;
        var skillComponentInfo = new SkillComponentInfo(user, user.Tile, user.Movement.DirX);
        if (skillComponents == null) return;
        
        foreach (var component in skillComponents)
        {
            component.Print(skillComponentInfo);
        }
    }

    public void Cancel(Unit user)
    {
        var skillComponentInfo = new SkillComponentInfo(user, user.Tile, user.Movement.DirX);
        foreach (var component in skillComponents)
        {
            component.Cancel(skillComponentInfo);
        }
    }
    
    public Skill(string name)
    {
        this.name = name;
    }

    public Skill DeepCopy()
    {
        var newSkill = new Skill(name)
        {
            Id = Id,
            label = label,
            spriteName = spriteName,
            description = description,
            elixir = elixir,
            castingTime = castingTime,
            executeCount = executeCount,
            isAnimation = isAnimation,
            upgradeCount = upgradeCount,
            skillComponents = skillComponents.ToList()
        };

        return newSkill;
    }
}
