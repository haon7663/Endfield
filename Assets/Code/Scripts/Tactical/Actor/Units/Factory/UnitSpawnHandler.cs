using System;
using Code.Scripts.Tactical.Actor.Tiles;
using DG.Tweening;
using UnityEngine;

public class UnitSpawnHandler : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(string unitName, Tile tile)
    {
        transform.position = tile.transform.position + offset;
        
        _spriteRenderer.DOFade(1, 0.5f).From(0).SetLoops(4, LoopType.Yoyo)
            .OnComplete(() => {
            SpawnManager.Inst.Summon(unitName, tile);
            Destroy(gameObject, 0.01f);
        });
    }
}
