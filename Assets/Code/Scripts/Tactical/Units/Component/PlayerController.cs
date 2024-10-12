using UnityEngine;

public class PlayerController : MonoBehaviour
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
        MoveInPut();
    }

    private void MoveInPut()
    {
        var dir = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.A))
            dir = Vector2.left;
        if (Input.GetKeyDown(KeyCode.D))
            dir = Vector2.right;
        if (Input.GetKeyDown(KeyCode.W))
            dir = Vector2.up;
        if (Input.GetKeyDown(KeyCode.S))
            dir = Vector2.down;

        if (dir != Vector2.zero && GridManager.Tiles.TryGetValue(_unit.Tile.Point + dir, out var tile))
        {
            _movement.OnMove(tile);
        }
    }
}
