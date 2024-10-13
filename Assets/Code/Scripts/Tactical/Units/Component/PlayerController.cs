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
        MoveInput();
    }

    private void MoveInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
            _movement.OnMove(GridManager.Inst.GetTile(_unit.Tile.Key - 1));
        if (Input.GetKeyDown(KeyCode.D))
            _movement.OnMove(GridManager.Inst.GetTile(_unit.Tile.Key + 1));
        
        if (Input.GetKeyDown(KeyCode.S))
            _movement.OnFlip(Mathf.Approximately(transform.localScale.x, 1));
    }
}
