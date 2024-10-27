using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Task _currentTask;
    private readonly Queue<Action> _inputBuffer = new Queue<Action>();
    
    private Movement _movement;
    private Unit _unit;
    private SkillHolder _skillHolder;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _unit = GetComponent<Unit>();
        _skillHolder = GetComponent<SkillHolder>();
    }

    private void Start()
    {
    }

    private void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(_skillHolder.Execute());
        }
    }

    private void BufferedInput(IEnumerator c)
    {
        Action action = () =>
        {
            _currentTask = new Task(c);
            _currentTask.Finished += manual =>
            {
                if (_inputBuffer.Count > 0)
                    _inputBuffer.Dequeue()?.Invoke();
            };
        };
        
        if (_currentTask is { Running: true })
        {
            if (_inputBuffer.Count < 1)
                _inputBuffer.Enqueue(action);
            return;
        }
        action.Invoke();
    }
    
    private void MoveInput(int key)
    {
        BufferedInput(_movement.OnMove(key));
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (ArtDirectionManager.Inst.onBulletTime)
                return;
            
            MoveInput(-1);
        }
    }
    
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (ArtDirectionManager.Inst.onBulletTime)
                return;
            
            MoveInput(1);
        }
    }
    
    public void OnFlip(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (ArtDirectionManager.Inst.onBulletTime)
                return;
            
            BufferedInput(_movement.OnFlip(Mathf.Approximately(transform.localScale.x, 1)));
        }
    }

    public void OnSkillUsed(InputAction.CallbackContext context)
    {
        var key = context.control.name;

        if (context.started)
        {
            ArtDirectionManager.Inst.StartBulletTime(new List<Unit> { _unit });
            switch (key)
            {
                case "u":
                    PrintSkill(0);
                    break;
                case "i":
                    PrintSkill(1);
                    break;
                case "j":
                    PrintSkill(2);
                    break;
                case "k":
                    PrintSkill(3);
                    break;
            }
        }

        if (context.canceled)
        {
            switch (key)
            {
                case "u":
                    BufferedInput(UseSkill(0));
                    break;
                case "i":
                    BufferedInput(UseSkill(1));
                    break;
                case "j":
                    BufferedInput(UseSkill(2));
                    break;
                case "k":
                    BufferedInput(UseSkill(3));
                    break;
            }
            ArtDirectionManager.Inst.EndBulletTime();
        }
    }

    private void PrintSkill(int skillNum)
    {
        var skill = SkillManager.Inst.GetSkillAtIndex(skillNum);
        
        if (skill == null || GameManager.Inst.curElixir < skill.elixir) return;
        skill.Print(_unit);
    }
    
    private IEnumerator UseSkill(int skillNum)
    {
        Debug.Log("attack");

        var skill = SkillManager.Inst.GetSkillAtIndex(skillNum);
        
        if (skill == null || GameManager.Inst.curElixir < skill.elixir) yield break;
        skill.Use(_unit);
        SkillManager.Inst.ConsumeSkill(skillNum);
        GameManager.Inst.curElixir -= skill.elixir;
        GridManager.Inst.RevertGrid(_unit);
        
        yield return new WaitForSeconds(0.2f);
    }
}
