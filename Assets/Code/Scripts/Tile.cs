using UnityEngine;

public class Tile : MonoBehaviour
{
    public int Key { get; private set; }

    public void Init(int key)
    {
        Key = key;
    }
}
