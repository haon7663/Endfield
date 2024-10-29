using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class DefeatPanelController : MonoBehaviour
{
    [SerializeField] private Panel panel;
    [SerializeField] private ClosePanel closePanel;
    [SerializeField] private InventoryController inventoryController;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            panel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
            closePanel.onClose += Hide;
        }
    }
    public void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true, 0.25f);
    }
}
