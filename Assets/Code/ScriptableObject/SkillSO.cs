using UnityEngine;

public class SkillSO : ScriptableObject
{
    public enum RangeType { OnlyX,Cross,Circle,Box } //OnlyX : x축만  Cross : 십자가 Circle : 원 Box : 사각형
    public enum AreaType {Single,Direction,Entire} //Single : 타일 하나만 Direction : 방향 Entire : 전체

    public RangeType rangeType;
    public AreaType areaType;
    public int range;
    public int priority;
    public int skillDamage;
    public int attackCount;
    public float skillCool;



}
