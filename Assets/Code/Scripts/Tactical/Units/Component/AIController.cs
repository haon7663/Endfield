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
        _unit.Tile.GetAreaInRange(2);
    }
}
