using UnityEngine;

public class QuickWalkComponent : SkillComponent
{
    public override void Execute(SkillComponentInfo info)
    {
        for (var i = 1; i <= distance; i++)
        {
            var tile = GetStartingTile(info, i);
            if (tile && tile.IsOccupied)
            {
                break;
            }
        }
    }

    public override void Print(SkillComponentInfo info)
    {
        
    }
    
    
    private int CalculateDistance(SkillComponentInfo info)
    {
        var index = 0;
        for (var i = 1; i <= distance; i++)
        {
            var tile = GetStartingTile(info, i);
            if (tile && tile.IsOccupied)
                break;
            index = i;
        }
        return index;
    }
}
