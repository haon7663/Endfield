using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    private Unit _unit;
    
    private readonly Queue<Action> _inputBuffer = new Queue<Action>();
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed;
    
    public Vector2 Dir => _dir;
    
    private Vector2 _dir = Vector2.right;
    private bool _isMove;
    
    private static readonly int IsFrontDash = Animator.StringToHash("isFrontDash");
    private static readonly int IsBackDash = Animator.StringToHash("isBackDash");
    private static readonly int IsFlip = Animator.StringToHash("isFlip");

    private void Start()
    {
        _unit = GetComponent<Unit>();
    }

    public void OnMove(int key)
    {
        if (_isMove)
        {
            if (_inputBuffer.Count < 1)
                _inputBuffer.Enqueue(() => OnMove(key));
            return;
        }
        MoveTo(GridManager.Inst.GetTile(_unit.Tile.Key + key));
    }
    
    public void MoveTo(Tile tile)
    {
        if (tile.IsOccupied) return;

        DOTween.Kill(this);
        
        var prevTile = _unit.Tile;
        var distance = tile.Key - prevTile.Key;
        var dir = distance > 0 ? Vector2.right : Vector2.left;
        var anim = dir == _dir ? IsFrontDash : IsBackDash;
        
        _isMove = true;
        animator.SetBool(anim, true);
        _unit.Place(tile);
        
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(tile.transform.position + Vector3.up * 0.5f, moveSpeed).SetEase(Ease.OutCirc))
            .Join(spriteRenderer.transform.DOLocalJump(Vector3.zero, 0.1f, 1, moveSpeed).SetEase(Ease.OutCirc))
            .OnComplete(() =>
            {
                animator.SetBool(anim, false);
                OnMoveEnd();
            });
    }

    public void OnFlip(bool isFlip)  //SpriteBillboard 때문에 FlipX 안되서 로컬 스케일로 구현함
    {
        if (_isMove)
        {
            if (_inputBuffer.Count < 1)
                _inputBuffer.Enqueue(() => OnFlip(isFlip));
            return;
        }
        
        _isMove = true;
        animator.SetBool(IsFlip, true);
        var sequence = DOTween.Sequence();
        sequence.Append(spriteRenderer.transform.DOLocalJump(Vector3.zero, 0.2f, 1, moveSpeed).SetEase(Ease.Linear))
            .InsertCallback(moveSpeed * 0.5f, () =>
            {
                _dir = isFlip ? Vector2.left : Vector2.right;
                transform.localScale = new Vector3(isFlip ? -1 : 1, 1, 1);
            })
            .OnComplete(() =>
            {
                animator.SetBool(IsFlip, false);
                OnMoveEnd();
            });
    }
    
    private void OnMoveEnd()
    {
        _isMove = false;

        if (_inputBuffer.Count > 0)
            _inputBuffer.Dequeue()?.Invoke();
    }
}
