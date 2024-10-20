using System;
using UnityEngine;

[Serializable]
public abstract class SkillComponent
{
    public string saveName;
    public abstract void Execute(Unit user);
    public abstract void Print(Unit user);
}
