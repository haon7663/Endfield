using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitSO", menuName = "Scriptable Objects/UnitSO")]
public class UnitSO : ScriptableObject
{
    public new string name;
    public float hp;
    public Sprite sprite;
    public Animator animator;
    public List<SkillSO> skills = new List<SkillSO>();
}
