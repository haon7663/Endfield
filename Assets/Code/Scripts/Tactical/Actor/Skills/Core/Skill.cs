using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        SkillComponent defaultComponent = null;
        foreach (var component in skillComponents) 
        {
            Debug.Log(component.saveName + "/ " + component.ExecuteType);
            switch (component.ExecuteType)
            {
                case SkillExecuteType.Default:
                    component.Execute(user);
                    defaultComponent = component;
                    yield return null;
                    break;
                case SkillExecuteType.OnHit:
                    defaultComponent?.AddOnHit(() => component.Execute(user));
                    break;
                case SkillExecuteType.OnEnd:
                    defaultComponent?.AddOnEnd(() => component.Execute(user));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        SkillManager.Inst.RevertSkillArea(user);
        Cancel(user);
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
