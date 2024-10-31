using DG.Tweening;
using UnityEngine;

public class UnitSpawnHandler : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    public void Init(string unitName, Tile tile)
    {
        transform.position = tile.transform.position + offset;
        DOVirtual.DelayedCall(1f, () =>
        {
            SpawnManager.Inst.Summon(unitName, tile);
            Destroy(gameObject);
        });
    }
}
