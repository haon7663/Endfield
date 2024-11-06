using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RelicSO", menuName = "Scriptable Objects/RelicSO")]
public class RelicSO : ScriptableObject
{
    public enum RelicType
    {
        HpRegen,MaxElixier,MaxHp,GetSkillUpgrade,GetGold
    }

    public RelicType relicType;
    public Sprite sprite;
    public string name;
    public string description;
    Action action;

    public void IncreaseHpRegenArtifact()
    {
        ArtifactManager.Inst.hpRegenArtifact++;
    }

    public void IncreaseSkillUpgradeArtifact()
    {
        ArtifactManager.Inst.skillUpgradeArtifact++;
    }

    public void IncreaseGoldArtifact()
    {
        ArtifactManager.Inst.goldArtifact++;
    }

    public void Execute()
    {
        switch (relicType)
        {
            case RelicType.HpRegen:
                action += IncreaseHpRegenArtifact;
                break;
            case RelicType.MaxElixier:
                action += () => ArtifactManager.Inst.IncreaseElixirMax();
                break;
            case RelicType.MaxHp:
                action += () => ArtifactManager.Inst.IncreaseHpMax();
                break;
            case RelicType.GetSkillUpgrade:
                action += IncreaseSkillUpgradeArtifact;
                break;
            case RelicType.GetGold:
                action += IncreaseGoldArtifact;
                break;
        }

        action?.Invoke();
        action = null; 
    }

}
