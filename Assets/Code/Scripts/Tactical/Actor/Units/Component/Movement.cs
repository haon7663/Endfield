using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Unit _unit;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    
    [SerializeField] private float moveSpeed;

    public bool isAnchored;
    
    public Vector2 Dir => _dir;
    public int DirX => (int)_dir.x;
    
    private Vector2 _dir = Vector2.right;
    private Tile _prevTile;
    
    private static readonly int IsFrontDash = Animator.StringToHash("isFrontDash");
    private static readonly int IsBackDash = Animator.StringToHash("isBackDash");
    private static readonly int IsFlip = Animator.StringToHash("isFlip");

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _spriteRenderer = _unit.SpriteTransform.GetComponent<SpriteRenderer>();
        _animator = _unit.SpriteTransform.GetComponent<Animator>();
        
        _prevTile = _unit.Tile;
    }

    public IEnumerator OnMove(int key)
    {
        var tile = GridManager.Inst.GetTile(_unit.Tile.Key + key);
        if (tile.IsOccupied) yield break;
        _unit.Place(tile);
        yield return StartCoroutine(MoveTo(tile));
    }

    public IEnumerator OnSwap(Unit other)
    {
        var prevTile = _unit.Tile;
        var targetTile = other.Tile;
        
        _unit.Swap(other);
        StartCoroutine(other.Movement.MoveTo(prevTile));
        yield return StartCoroutine(MoveTo(targetTile));
    }
    
    public IEnumerator MoveTo(Tile tile)
    {
        DOTween.Kill(this);
        
        var distance = tile.Key - _prevTile.Key;
        var dir = distance > 0 ? Vector2.right : Vector2.left;
        var anim = dir == _dir ? IsFrontDash : IsBackDash;
        _prevTile = tile;
        
        _animator.SetBool(anim, true);
        
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(tile.transform.position + Vector3.up * 0.5f, moveSpeed).SetEase(Ease.OutCirc))
            .Join(_spriteRenderer.transform.DOLocalJump(Vector3.zero, 0.1f, 1, moveSpeed).SetEase(Ease.OutCirc))
            .OnComplete(() => _animator.SetBool(anim, false));
        
        yield return sequence.WaitForCompletion();
    }

    public IEnumerator OnFlip(bool isFlip) 
    {
        /*_animator.SetBool(IsFlip, true);
        var sequence = DOTween.Sequence();
        sequence.Append(_spriteRenderer.transform.DOLocalJump(Vector3.zero, 0.2f, 1, moveSpeed).SetEase(Ease.Linear))
            .InsertCallback(moveSpeed * 0.5f, () =>
            {
                _dir = isFlip ? Vector2.left : Vector2.right;
                transform.localScale = new Vector3(isFlip ? -1 : 1, 1, 1);
            })
            .OnComplete(() => _animator.SetBool(IsFlip, false) );

        yield return sequence.WaitForCompletion();*/
        
        _dir = isFlip ? Vector2.left : Vector2.right;
        transform.localScale = new Vector3(isFlip ? -1 : 1, 1, 1);
        _unit.OnAction?.Invoke();
        yield break;
    }
}
