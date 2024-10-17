using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum RangeType { Range, Hover, Always } //Range: 거리 전체 - Hover: 일정 거리 - Always: 상시
public enum SelectType { Direction, Entire } //Direction: 방향 - Entire: 전체

public class SkillSO : ScriptableObject
{
    public new string name;
    public int elixir;
    public float castingTime = 0.5f;
    
    [Header("인공지능")]
    public int priority;
}
