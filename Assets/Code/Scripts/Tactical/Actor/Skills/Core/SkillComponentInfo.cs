using System;
using Code.Scripts.Tactical.Actor.Tiles;

[Serializable]
public class SkillComponentInfo
{
    public Unit user;
    public Tile tile;
    public int dirX;

    public SkillComponentInfo(Unit user, Tile tile, int dirX)
    {
        this.user = user;
        this.tile = tile;
        this.dirX = dirX;
    }

    public SkillComponentInfo(SkillComponentInfo info, Tile tile)
    {
        user = info.user;
        this.tile = tile;
        dirX = info.dirX;
    }
}