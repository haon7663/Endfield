using System;

[Serializable]
public class SkillPreset
{
    public SkillComponentType type;

    [DrawIf("type", SkillComponentType.Attack)]
    public AttackComponent attackComponent;
    [DrawIf("type", SkillComponentType.Move)]
    public MoveComponent moveComponent;
    [DrawIf("type", SkillComponentType.Dash)]
    public DashComponent dashComponent;
}