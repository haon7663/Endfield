using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [SerializeField] private Panel panel;
    [SerializeField] private ClosePanel closePanel;
    [SerializeField] private KeyCode keycode;
    private bool _isActive = false;

  

    private void Start()
    {
        Show();

    }



    public void Show()
    {
        panel.SetPosition(PanelStates.Show, true,0.5f);
        _isActive = true;
        closePanel.onClose += ()=> Hide();
    }

    public void Hide()
    {
        SceneManager.LoadScene("Design");
        _isActive = false;
    }
}
