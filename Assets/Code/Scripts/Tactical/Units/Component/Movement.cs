using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveSpeed;
    
    private Unit _unit;
    private bool _isMove;

    private void Start()
    {
        _unit = GetComponent<Unit>();
    }
    
    public void OnMove(Tile tile)
    {
        if (_isMove || tile.IsOccupied) return;
        
        var sequence = DOTween.Sequence();

        _isMove = true;
        sequence.Append(transform.DOMove(tile.transform.position + Vector3.up * 0.5f, moveSpeed))
            .Join(spriteRenderer.transform.DOLocalJump(Vector3.zero, 0.25f, 1, moveSpeed))
            .OnComplete(() =>
            {
                _unit.Place(tile);
                _isMove = false;
            });
    }

    public void OnFlip(bool isFlip)  //SpriteBillboard 때문에 FlipX 안되서 로컬 스케일로 구현함
    {
        transform.localScale = new Vector3(isFlip?-1:1,1, 1);
    }
}
