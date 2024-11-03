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

    public abstract void Execute(SkillComponentInfo info);
    public abstract void Print(SkillComponentInfo info);

    public virtual void Cancel(SkillComponentInfo info)
    {
        executeObjects.Clear();
    }
    
    protected Tile GetStartingTile(SkillComponentInfo info, int index = 0)
    {
        return GridManager.Inst.TryGetTile(info.tile.Key + index * info.dirX, out var tile) ? tile : null;
    }

    protected List<ISkillExecuter> executeObjects = new List<ISkillExecuter>();
    public void AddOnHit(Action<SkillComponentInfo> onHit)
    {
        foreach (var executeObject in executeObjects)
        {
            executeObject.OnHit += onHit;
        }
    }
    public void AddOnEnd(Action<SkillComponentInfo> onEnd)
    {
        foreach (var executeObject in executeObjects)
        {
            executeObject.OnEnd += onEnd;
        }
    }
}