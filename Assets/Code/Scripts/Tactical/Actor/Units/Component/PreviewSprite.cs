using UnityEngine;

public class PreviewSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Init(int key, Sprite sprite)
    {
        transform.position = GridManager.Inst.GetTile(key).transform.position + Vector3.up * 0.5f;
        spriteRenderer.sprite = sprite;
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }
}