using UnityEngine;

public class SkillPanelController : MonoBehaviour
{
    [SerializeField] private SkillPanel[] skillPanels = new SkillPanel[4];
    [SerializeField] private SkillPanel prevSkillPanel;

    public void UpdatePanels(Skill[] skills, Skill prevSkill)
    {
        for (var i = 0; i < 4; i++)
        {
            skillPanels[i].Init(skills[i]);
        }
        prevSkillPanel.Init(prevSkill);
    }
}
