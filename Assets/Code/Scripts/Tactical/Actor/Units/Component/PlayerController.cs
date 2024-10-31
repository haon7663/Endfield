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

    private int _skillNum;
    private float _skillHoldTime;
    private bool _isSkillHolding;
    private bool _isPrinted;
    
    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _unit = GetComponent<Unit>();
        _skillHolder = GetComponent<SkillHolder>();
    }

    private void Update()
    {
        if (!_isSkillHolding || _isPrinted) return;
        _skillHoldTime += Time.deltaTime;
        
        if (!(_skillHoldTime > 0.2f)) return;
        BufferedInput(PrintSkill(_skillNum));
        _isPrinted = true;
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

    private void SwapInput(Unit other)
    {
        BufferedInput(_movement.OnSwap(other));
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (ArtDirectionManager.Inst.onBulletTime)
                return;

            var targetTile = GridManager.Inst.GetTile(_unit.Tile.Key - 1);
            if (targetTile.IsOccupied)
                SwapInput(targetTile.content);
            else
                MoveInput(-1);
        }
    }
    
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (ArtDirectionManager.Inst.onBulletTime)
                return;
            
            var targetTile = GridManager.Inst.GetTile(_unit.Tile.Key + 1);
            if (targetTile.IsOccupied)
                SwapInput(targetTile.content);
            else
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
            if (!_isSkillHolding)
            {
                _skillNum = key switch
                {
                    "u" => 0,
                    "i" => 1,
                    "j" => 2,
                    "k" => 3,
                    _ => -1
                };
            
                var skill = SkillManager.Inst.GetSkillAtIndex(_skillNum);
                if (GameManager.Inst.curElixir >= skill.elixir)
                {
                    _skillHoldTime = 0;
                    _isSkillHolding = true;
                }
            }
        }

        if (context.canceled)
        {
            ArtDirectionManager.Inst.EndBulletTime();
            if (!_isSkillHolding)
                return;
            
            BufferedInput(UseSkill(_skillNum));
        }
    }

    private IEnumerator PrintSkill(int skillNum)
    {
        var skill = SkillManager.Inst.GetSkillAtIndex(skillNum);

        if (skill == null) yield break;
        if (GameManager.Inst.curElixir < skill.elixir)
        {
            TextHudController.Inst.ShowElixirConsume(_unit.transform.position + Vector3.up * 1.5f, skill.elixir);
            yield break;
        }
        
        ArtDirectionManager.Inst.StartBulletTime(new List<Unit> { _unit });
        SkillManager.Inst.ApplySkillArea(_unit, skill);
    }
    
    private IEnumerator UseSkill(int skillNum)
    {
        var skill = SkillManager.Inst.GetSkillAtIndex(skillNum);
        
        if (skill == null || GameManager.Inst.curElixir < skill.elixir) yield break;
        StartCoroutine(skill.Use(_unit));
        SkillManager.Inst.ConsumeSkill(skillNum);
        GameManager.Inst.curElixir -= skill.elixir;
        
        _isSkillHolding = false;
        _skillHoldTime = 0;
        _isPrinted = false;
        
        yield return new WaitForSeconds(0.2f);
    }
}
