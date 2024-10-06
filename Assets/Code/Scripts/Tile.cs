using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2 Point { get; private set; }

    public void Init(Vector2 key)
    {
        Point = key;
    }
}
