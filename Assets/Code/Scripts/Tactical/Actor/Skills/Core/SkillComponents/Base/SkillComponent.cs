using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SkillComponent
{
    public string executeType;
    public string saveName;
    public int distance;
    public SkillExecuteType ExecuteType => Enum.TryParse(executeType, out SkillExecuteType result) ? result : SkillExecuteType.Default;

    public abstract void Execute(Unit user);
    public abstract void Print(Unit user);
    public virtual void Cancel(Unit user) { }
    
    protected Tile GetStartingTile(Unit user, int index = 0)
    {
        return GridManager.Inst.GetTile(user.Tile.Key + user.additionalKey + index * user.Movement.DirX);
    }

    protected List<IExecuteAble> ExecuteObjects = new List<IExecuteAble>();
    public void AddOnHit(Action onHit)
    {
        foreach (var executeObject in ExecuteObjects)
        {
            executeObject.OnHit += onHit;
        }
    }
    public void AddOnEnd(Action onEnd)
    {
        foreach (var executeObject in ExecuteObjects)
        {
            Debug.Log("AddOnEnd");
            executeObject.OnEnd += onEnd;
        }
    }
}