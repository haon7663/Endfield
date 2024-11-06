using DG.Tweening;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.SceneManagement;

public class EndingPanelController : MonoBehaviour
{
    [SerializeField] private Panel panel;

    public void Show()
    {
        panel.SetPosition(PanelStates.Show, true, 2.5f, Ease.InCubic);
    }
}