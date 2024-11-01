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
        var skillComponentInfo = new SkillComponentInfo(user, user.Tile, user.Movement.DirX);
        foreach (var component in skillComponents) 
        {
            Debug.Log(component.saveName + "/ " + component.ExecuteType);
            switch (component.ExecuteType)
            {
                case SkillExecuteType.Default:
                    component.Execute(skillComponentInfo);
                    defaultComponent = component;
                    break;
                case SkillExecuteType.OnHit:
                    defaultComponent?.AddOnHit(component.Execute);
                    break;
                case SkillExecuteType.OnEnd:
                    defaultComponent?.AddOnEnd(component.Execute);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        SkillManager.Inst.RevertSkillArea(user);
        Cancel(user);

        yield return new WaitForSeconds(0.2f);
    }

    public void Print(Unit user)
    {
        user.additionalKey = 0;
        var skillComponentInfo = new SkillComponentInfo(user, user.Tile, user.Movement.DirX);
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
}
