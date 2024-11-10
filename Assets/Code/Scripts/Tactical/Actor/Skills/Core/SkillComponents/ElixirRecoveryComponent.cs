using UnityEngine;

public class ElixirRecoveryComponent : SkillComponent
{
    public float value;

    public override void Execute(SkillComponentInfo info)
    {
        GameManager.Inst.curElixir += value;
    }

    public override void Print(SkillComponentInfo info) { }
}
