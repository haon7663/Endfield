using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Tactical.Actor.Tiles
{
    public class Tile : MonoBehaviour
    {
        public int Key { get; private set; }
        public bool IsOccupied => content;

        public Unit content;

        [SerializeField] private List<SpriteRenderer> lineRenderers;
        [SerializeField] private SpriteRenderer backGroundRenderer;

        public void Init(int key)
        {
            Key = key;
        }
    
        public void SetDefaultColor()
        {
            foreach (var lineRenderer in lineRenderers)
            {
                lineRenderer.color = Color.white;
            }
            backGroundRenderer.color = new Color(0, 0, 0, 0.25f);
        }

        public void SetColor(Color color)
        {
            foreach (var lineRenderer in lineRenderers)
            {
                lineRenderer.color = color;
            }
            backGroundRenderer.color = new Color(color.r, color.g, color.b, 0.25f);
        }
    }
}
