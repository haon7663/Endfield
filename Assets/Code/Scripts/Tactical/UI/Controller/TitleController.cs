using System;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    [SerializeField] private Panel panel;
    [SerializeField] private KeyCode keycode;
    private bool _isActive = false;

    private void Update()
    {
        if (_isActive)
        {
            if (Input.GetKeyDown(keycode))
            {
                Hide();
            }
        }
    }

    public void Show()
    {
        panel.SetPosition(PanelStates.Show, true,0.5f);
        _isActive = true;
    }

    public void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true,0.5f);
        _isActive = false;
    }
}
