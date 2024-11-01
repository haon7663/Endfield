using System;

public interface ISkillExecuter
{
    Action<SkillComponentInfo> OnHit { get; set; }
    Action<SkillComponentInfo> OnEnd { get; set; }
}