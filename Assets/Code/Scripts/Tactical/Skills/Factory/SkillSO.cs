using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum RangeType { Range, Hover, Always } //Range: 거리 전체 - Hover: 일정 거리 - Always: 상시
public enum SelectType { Direction, Entire } //Direction: 방향 - Entire: 전체

public class SkillSO : ScriptableObject
{
    public RangeType rangeType;
    public SelectType selectType;
    
    [Header("능력치")]
    public int range = 1;
    public int damage = 5;
    public int attackCount = 1;
    public float castingTime = 0.5f;
    public Sprite skillImage;
    
    [Header("인공지능")]
    public int priority;
}
