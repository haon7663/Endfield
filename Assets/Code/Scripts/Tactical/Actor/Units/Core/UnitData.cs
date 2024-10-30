using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
    public string Name;
    public RuntimeAnimatorController AnimatorController;
    public int Health;
    public float ActionTime;
    public List<Skill> Skills;

    public UnitData(string name, RuntimeAnimatorController animatorController, int health, float actionTime, List<Skill> skills)
    {
        Name = name;
        AnimatorController = animatorController;
        Health = health;
        ActionTime = actionTime;
        Skills = skills;
    }
}