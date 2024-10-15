using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    public Vector2 Dir => _dir;
    private Vector2 _dir = Vector2.right;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed;
    
    private Unit _unit;
    
    private bool _isMove;
    
    private static readonly int IsFrontDash = Animator.StringToHash("isFrontDash");
    private static readonly int IsBackDash = Animator.StringToHash("isBackDash");

    private void Start()
    {
        _unit = GetComponent<Unit>();
    }
    
    public void OnMove(Tile tile)
    {
        if (_isMove || tile.IsOccupied) return;

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
                _isMove = false;
                animator.SetBool(anim, false);
            });
    }

    public void OnFlip(bool isFlip)  //SpriteBillboard 때문에 FlipX 안되서 로컬 스케일로 구현함
    {
        _dir = isFlip ? Vector2.left : Vector2.right;
        transform.localScale = new Vector3(isFlip ? -1 : 1, 1, 1);
    }
}
