using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Movement _movement;
    private Unit _unit;
    private SkillHolder _skillHolder;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _unit = GetComponent<Unit>();
        _skillHolder = GetComponent<SkillHolder>();
    }

    private void Update()
    { 
        MoveInput();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(_skillHolder.Execute());
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            _skillHolder.AddCastingViewer(SkillManager.Inst.GetSkillAtIndex(0));
            TimeCaster.SetTimeScale(0.02f);
            GameManager.Inst.BSVolume(true);
        }
        if (Input.GetKeyDown(KeyCode.I))
            _skillHolder.AddCastingViewer(SkillManager.Inst.GetSkillAtIndex(1));
        if (Input.GetKeyDown(KeyCode.J))
            _skillHolder.AddCastingViewer(SkillManager.Inst.GetSkillAtIndex(2));
        if (Input.GetKeyDown(KeyCode.K))
            _skillHolder.AddCastingViewer(SkillManager.Inst.GetSkillAtIndex(3));
    }

    private void MoveInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
            _movement.OnMove(-1);
        if (Input.GetKeyDown(KeyCode.D))
            _movement.OnMove(1);
        
        if (Input.GetKeyDown(KeyCode.S))
            _movement.OnFlip(Mathf.Approximately(transform.localScale.x, 1));
    }
}
