using System.Collections.Generic;
using GDX.Collections.Generic;
using UnityEngine;

public class SkillPanelController : MonoBehaviour
{
    [SerializeField] private Transform[] skillFrames;
    [SerializeField] private Transform prevSkillFrame;

    [SerializeField] private SkillPanel skillPanelPrefab;
    [SerializeField] private Transform skillPanelParent;
    
    private SerializableDictionary<int, SkillPanel> _indexToSkillPanels = new SerializableDictionary<int, SkillPanel>();

    public void UpdatePanels(Skill[] skills, Skill prevSkill)
    {
        /*for (var i = 0; i < 3; i++)
        {
            var skill = skills[i];
            if (!_indexToSkillPanels[i])
            {
                var skillPanel = Instantiate(skillPanelPrefab, skillPanelParent);
                skillPanel.Init(skill);
                _indexToSkillPanels[i] = skillPanel;
            }
            
        }
        prevSkillPanel.Init(prevSkill);*/
    }
}
