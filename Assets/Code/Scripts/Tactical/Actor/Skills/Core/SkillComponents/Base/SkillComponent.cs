using System;
using UnityEngine;

[Serializable]
public abstract class SkillComponent
{
    public string saveName;
    public int distance;
    public abstract void Execute(Unit user);
}
