using System;
using UnityEngine;

[Serializable]
public abstract class SkillComponent
{
    public string saveName;
    public int distance;
    public abstract void Execute(Unit user);
    public abstract void Print(Unit user);
    public virtual void Cancel(Unit user) { }
    
    protected Tile GetStartingTile(Unit user, int index = 0)
    {
        return GridManager.Inst.GetTile(user.Tile.Key + user.additionalKey + index * user.Movement.DirX);
    }
}
