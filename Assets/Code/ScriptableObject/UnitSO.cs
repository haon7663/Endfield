using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitSO", menuName = "Scriptable Objects/UnitSO")]
public class UnitSO : ScriptableObject
{
    public string unitName;
    public float unitHp;
    public Sprite unitSprite;
    public Animator unitAnimator;
    public List<SkillSO> unitSkills = new List<SkillSO>();
}
