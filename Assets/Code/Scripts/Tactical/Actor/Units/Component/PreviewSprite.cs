using UnityEngine;

public class PreviewSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Init(int key, Sprite sprite, int dirX)
    {
        transform.position = GridManager.Inst.GetTile(key).transform.position + Vector3.up * 0.5f;
        spriteRenderer.sprite = sprite;
        transform.localScale = new Vector3(dirX, 1, 1);
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }
}