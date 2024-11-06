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

    public void Excute()
    {
        switch (relicType)
        {
            case RelicType.HpRegen:
              action +=()=>  ArtifactManager.Inst.hpRegenArtifact++;
                break;
            case RelicType.MaxElixier:
                action += () => ArtifactManager.Inst.IncreaseElixirMax();
                break;
            case RelicType.MaxHp:
                action += () => ArtifactManager.Inst.IncreaseHpMax();
                break;
            case RelicType.GetSkillUpgrade:
                action += () => ArtifactManager.Inst.skillUpgradeArtifact++;
                break;
            case RelicType.GetGold:
                action += () => ArtifactManager.Inst.goldArtifact++;
                break;
        }

        action?.Invoke();
    }

}
