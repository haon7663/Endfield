using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    [SerializeField] private Image image;
    
    public Skill data;

    public void Init(Skill skill)
    {
        data = skill;
        image.sprite = SkillLoader.GetSkillSprite(skill.name);
    }
}
