using UnityEngine;

public class Tile : MonoBehaviour
{
    public int Key { get; private set; }
    public bool IsOccupied => content;

    public Unit content;

    public void Init(int key)
    {
        Key = key;
    }
}
