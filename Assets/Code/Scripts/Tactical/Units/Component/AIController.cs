using UnityEngine;

public class AIController : MonoBehaviour
{
    private Movement _movement;
    private Unit _unit;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _unit = GetComponent<Unit>();
    }

    private void Update()
    {
        EnemyMove();
    }
    
    private void EnemyMove() // 적 -> 플레이어쪽으로 이동
    {
        var target = GameManager.Inst.player;
        float direction = target.position.x - transform.position.x;
        int targetTileIndex = direction > 0 ? _unit.Tile.Key + 1 : _unit.Tile.Key - 1;
        _movement.OnMove(GridManager.Inst.GetTile(targetTileIndex));
    }
}
