using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Unit _unit;
    private bool _isMove;

    private void Start()
    {
        _unit = GetComponent<Unit>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnMove(GridManager.Tiles[_unit.Tile.Point + Vector2.left]);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnMove(GridManager.Tiles[_unit.Tile.Point + Vector2.right]);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnMove(GridManager.Tiles[_unit.Tile.Point + Vector2.down]);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnMove(GridManager.Tiles[_unit.Tile.Point + Vector2.up]);
        }
    }

    public void OnMove(Tile tile)
    {
        if (_isMove) return;
        
        var sequence = DOTween.Sequence();

        _isMove = true;
        sequence.Append(transform.DOMove(tile.transform.position + Vector3.up * 0.5f, 0.25f))
            .Join(spriteRenderer.transform.DOLocalJump(Vector3.zero, 0.25f, 1, 0.3f))
            .OnComplete(() =>
            {
                _unit.Place(tile);
                _isMove = false;
            });
    }
}
