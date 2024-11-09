using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillHolder : MonoBehaviour
{
    private Unit _unit;
    private Animator _animator;

    public List<Skill> skills;

    public SkillHoldPanel skillHoldPanel;
    
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int IsReady = Animator.StringToHash("isReady");

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _animator = _unit.SpriteTransform.GetComponent<Animator>();
        
        skillHoldPanel = SkillHoldPanelController.Inst.Connect(this);
    }

    //모든 스킬 방출
    public IEnumerator Execute()
    {
        var viewers = SkillHoldPanelController.Inst.GetViewers(this).ToList();
        foreach (var castingViewer in viewers.Where(castingViewer => castingViewer.Data != null))
        {
            SkillManager.Inst.ApplySkillArea(_unit, castingViewer.Data);

            _animator.SetBool(IsReady, true);
            yield return StartCoroutine(castingViewer.Cast());
            _animator.SetBool(IsReady, false);

            var skill = castingViewer.Data;
            
            for (var i = 0; i < skill.executeCount; i++)
            {
                if (skill.isAnimation == 1)
                    _animator.SetTrigger(Attack);
                skill.Use(_unit);
                yield return new WaitForSeconds(0.2f / skill.executeCount);
            }
            
            castingViewer.Remove();
        }
    }

    public void AddViewer(Skill skill)
    {
        SkillHoldPanelController.Inst.AddSkill(this, skill);
    }
}
