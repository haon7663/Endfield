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

    private void Update()
    {


        //Test
        if(_unit.role == Role.player)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnMove(GridManager.Inst.GetTile(_unit.Tile.Key - 1));
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                OnMove(GridManager.Inst.GetTile(_unit.Tile.Key + 1));
            }
        }
        

        if (Input.GetKeyDown(KeyCode.F1) && _unit.role == Role.enemy)
        {
            EnemyMove();
        }


    }

    void EnemyMove() // 적 -> 플레이어쪽으로 이동
    {
        Transform target = GameManager.Inst.player;
        float direaction = target.position.x - transform.position.x;
        int targetTileIndex = direaction > 0 ? _unit.Tile.Key + 1 : _unit.Tile.Key - 1;
        OnMove(GridManager.Inst.GetTile(targetTileIndex));
    }

    



    public void OnMove(Tile tile)
    {
        if (_isMove) return;
        
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
}
