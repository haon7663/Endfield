using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    private Unit _unit;
    
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    
    [SerializeField] private float moveSpeed;
    
    public Vector2 Dir => _dir;
    public int DirX => (int)_dir.x;
    
    private Vector2 _dir = Vector2.right;
    
    private static readonly int IsFrontDash = Animator.StringToHash("isFrontDash");
    private static readonly int IsBackDash = Animator.StringToHash("isBackDash");
    private static readonly int IsFlip = Animator.StringToHash("isFlip");

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _spriteRenderer = _unit.SpriteTransform.GetComponent<SpriteRenderer>();
        _animator = _unit.SpriteTransform.GetComponent<Animator>();
    }

    public IEnumerator OnMove(int key)
    {
        yield return StartCoroutine(MoveTo(GridManager.Inst.GetTile(_unit.Tile.Key + key)));
    }
    
    public IEnumerator MoveTo(Tile tile)
    {
        DOTween.Kill(this);
        
        var prevTile = _unit.Tile;
        var distance = tile.Key - prevTile.Key;
        var dir = distance > 0 ? Vector2.right : Vector2.left;
        var anim = dir == _dir ? IsFrontDash : IsBackDash;
        
        _animator.SetBool(anim, true);
        _unit.Place(tile);
        
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(tile.transform.position + Vector3.up * 0.5f, moveSpeed).SetEase(Ease.OutCirc))
            .Join(_spriteRenderer.transform.DOLocalJump(Vector3.zero, 0.1f, 1, moveSpeed).SetEase(Ease.OutCirc))
            .OnComplete(() => _animator.SetBool(anim, false));
        
        yield return sequence.WaitForCompletion();
    }

    public IEnumerator OnFlip(bool isFlip) 
    {
        _animator.SetBool(IsFlip, true);
        var sequence = DOTween.Sequence();
        sequence.Append(_spriteRenderer.transform.DOLocalJump(Vector3.zero, 0.2f, 1, moveSpeed).SetEase(Ease.Linear))
            .InsertCallback(moveSpeed * 0.5f, () =>
            {
                _dir = isFlip ? Vector2.left : Vector2.right;
                transform.localScale = new Vector3(isFlip ? -1 : 1, 1, 1);
            })
            .OnComplete(() => _animator.SetBool(IsFlip, false) );

        yield return sequence.WaitForCompletion();
    }
}
