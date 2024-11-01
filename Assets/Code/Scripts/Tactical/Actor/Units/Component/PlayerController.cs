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
    private Animator _animator;

    private Skill _prevSkill;
    private int _skillNum = -1;
    
    private static readonly int Attack = Animator.StringToHash("attack");

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _unit = GetComponent<Unit>();
        _animator = _unit.SpriteTransform.GetComponent<Animator>();
    }

    private void Update()
    {
        /*if (!_isSkillHolding) return;
        
        _skillHoldTime += Time.deltaTime;

        if (!_isPrinted)
        {
            BufferedInput(PrintSkill(_skillNum));
            _isPrinted = true;
        }
        
        if (!(_skillHoldTime > 0.2f) || ArtDirectionManager.Inst.onBulletTime) return;
        ArtDirectionManager.Inst.StartBulletTime(new List<Unit> { _unit });*/
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
            {
                if (_movement.DirX != -1)
                    BufferedInput(_movement.OnFlip(true));
                return;
            }

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
            {
                if (_movement.DirX != 1)
                    BufferedInput(_movement.OnFlip(false));
                return;
            }
            
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

    #region SkillInput
    public void OnFirstSkill(InputAction.CallbackContext context)
    {
        if (context.started)
            OnSkillStarted(0);

        if (context.canceled)
            OnSkillCanceled(0);
    }
    public void OnSecondSkill(InputAction.CallbackContext context)
    {
        if (context.started)
            OnSkillStarted(1);
        
        if (context.canceled)
            OnSkillCanceled(1);
    }
    public void OnThirdSkill(InputAction.CallbackContext context)
    {
        if (context.started)
            OnSkillStarted(2);
        
        if (context.canceled)
            OnSkillCanceled(2);
    }
    public void OnFourthSkill(InputAction.CallbackContext context)
    {
        if (context.started)
            OnSkillStarted(3);
        
        if (context.canceled)
            OnSkillCanceled(3);
    }
    #endregion

    private void OnSkillStarted(int skillNum)
    {
        if (skillNum == _skillNum) return;
        _skillNum = skillNum;
            
        var skill = SkillManager.Inst.GetSkillAtIndex(_skillNum);
        if (GameManager.Inst.curElixir >= skill.elixir)
        {
            if (!ArtDirectionManager.Inst.onBulletTime)
                ArtDirectionManager.Inst.StartBulletTime(new List<Unit> { _unit });
            
            SkillManager.Inst.RevertSkillArea(_unit);
            _prevSkill?.Cancel(_unit);
            _prevSkill = skill;
            SkillManager.Inst.ApplySkillArea(_unit, skill);
        }
        else
        {
            TextHudController.Inst.ShowElixirConsume(_unit.transform.position + Vector3.up * 1.5f, skill.elixir);
        }
    }
    
    private void OnSkillCanceled(int skillNum)
    {
        if (skillNum != _skillNum) return;
        
        BufferedInput(UseSkill(skillNum));
        _skillNum = -1;
    }
    
    private IEnumerator UseSkill(int skillNum)
    {
        var skill = SkillManager.Inst.GetSkillAtIndex(skillNum);
        if (skill == null || GameManager.Inst.curElixir < skill.elixir) yield break;
        
        _animator.SetTrigger(Attack);
        
        SkillManager.Inst.ConsumeSkill(skillNum);
        GameManager.Inst.curElixir -= skill.elixir;
        ArtDirectionManager.Inst.EndBulletTime();
        
        yield return StartCoroutine(skill.Use(_unit));
    }
}
