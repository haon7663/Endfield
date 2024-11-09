using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class SkillHoldPanelController : Singleton<SkillHoldPanelController>
{
    [SerializeField] private SkillHoldPanel skillHoldPanelPrefab;
    [SerializeField] private Transform canvas;

    [SerializeField] private Vector3 offset;
    
    private Dictionary<SkillHolder, SkillHoldPanel> _skillHolderDict = new Dictionary<SkillHolder, SkillHoldPanel>();

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        foreach (var skillHolderToPanel in _skillHolderDict)
        {
            skillHolderToPanel.Value.transform.position = _mainCamera.WorldToScreenPoint(skillHolderToPanel.Key.transform.position + offset);
        }
    }

    public SkillHoldPanel Connect(SkillHolder target)
    {
        var skillHoldPanel = Instantiate(skillHoldPanelPrefab, canvas);
        _skillHolderDict.Add(target, skillHoldPanel);

        target.GetComponent<Health>().onDeath += () => Remove(target);

        return skillHoldPanel;
    }

    public void AddSkill(SkillHolder target, Skill skill)
    {
        if (_skillHolderDict.TryGetValue(target, out var skillHoldPanel))
        {
            skillHoldPanel.AddSkill(skill);
        }
    }

    public List<SkillCastingViewer> GetViewers(SkillHolder target)
    {
        return _skillHolderDict[target].SkillCastingViewers;
    }

    public void Remove(SkillHolder target)
    {
        _skillHolderDict[target].DOKill();
        Destroy(_skillHolderDict[target].gameObject);
        _skillHolderDict.Remove(target);
    }
}
