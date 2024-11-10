using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    [SerializeField] private GameObject settingCanvas,keyLayoutCanvas;
    [SerializeField] private Panel panel;
    [SerializeField] private ClosePanel closePanel;

    [SerializeField] private TextMeshProUGUI bgmText,seText;
    

    [SerializeField] private Slider bgmSlider, seSlider;
    public float _bgmVolume, _seVolume;
    private bool _isShown;

    private void Start()
    {
        ChangeSFXVolume();
        ChangeBGMVolume();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isShown)
        {
            Show();
        }
    }

    public void ChangeBGMVolume()
    {
        _bgmVolume = Mathf.FloorToInt(bgmSlider.value * 100);
        bgmText.text = $"- {_bgmVolume} % -";
    }

    public void ChangeSFXVolume()
    {
        _seVolume = Mathf.FloorToInt(seSlider.value * 100);
        seText.text = $"- {_seVolume} % -";
    }
    
    public void Show()
    {
        Time.timeScale = 0;
        GameManager.Inst.isGameActive = false;
        
        _isShown = true;
        panel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
        closePanel.onClose += Hide;
    }
    public void Hide()
    {
        Time.timeScale = 1;
        GameManager.Inst.isGameActive = true;
        
        _isShown = false;
        panel.SetPosition(PanelStates.Hide, true, 0.25f);
    }

    private void HideKeyLayout()
    {
        closePanel.onClose =null;
        keyLayoutCanvas.SetActive(false);
        settingCanvas.SetActive(true);

        DOVirtual.DelayedCall(0.5f, ()=>closePanel.onClose += Hide);
    }
    

    public void ActiveKeyLayoutCanvas()
    {
        Debug.Log("키레이아웃인");
        closePanel.onClose -= Hide;
        closePanel.onClose += HideKeyLayout;
        var delegates = closePanel.onClose.GetInvocationList();
        foreach (var del in delegates)
        {
            Debug.Log(del.Method.Name);
        }
        settingCanvas.SetActive(false);
        keyLayoutCanvas.SetActive(true);
    }

    public void ChangeKeyLayout()  //키 레이아웃 변경 할 때
    {
        
    }
}
