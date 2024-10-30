using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
    public string name;
    public RuntimeAnimatorController animatorController;
    public int health;
    public float actionTime;
    public List<Skill> skills;

    public UnitData(string name, RuntimeAnimatorController animatorController, int health, float actionTime, List<Skill> skills)
    {
        this.name = name;
        this.animatorController = animatorController;
        this.health = health;
        this.actionTime = actionTime;
        this.skills = skills;
    }
}