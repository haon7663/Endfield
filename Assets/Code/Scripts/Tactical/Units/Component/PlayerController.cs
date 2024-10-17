using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Movement _movement;
    private Unit _unit;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _unit = GetComponent<Unit>();
    }

    private void Update()
    { 
        MoveInput();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var skill = SkillLoader.GetSkills("skill").FirstOrDefault(skill => skill.name == "TestProjectile");
            skill?.Use(_unit);
        }

        if (Input.GetKeyDown(KeyCode.U))
            SkillManager.Inst.GetSkillAtIndex(0)?.Use(_unit);
        if (Input.GetKeyDown(KeyCode.I))
            SkillManager.Inst.GetSkillAtIndex(1)?.Use(_unit);
        if (Input.GetKeyDown(KeyCode.J))
            SkillManager.Inst.GetSkillAtIndex(2)?.Use(_unit);
        if (Input.GetKeyDown(KeyCode.K))
            SkillManager.Inst.GetSkillAtIndex(3)?.Use(_unit);
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
