using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Sequences;

public class TitleController : MonoBehaviour
{
    [SerializeField] private Panel panel;
    [SerializeField] private ClosePanel closePanel;
    [SerializeField] private KeyCode keycode;
    [SerializeField] private string sceneName;
    
    private bool _isSceneMoved;

    private void Start()
    {
        closePanel.onClose += Hide;
    }

    private void Hide()
    {
        if (_isSceneMoved) return;
        
        Fade.Inst.FadeOut(sceneName);
        _isSceneMoved = true;
    }
}