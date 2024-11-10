using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    
    
    [SerializeField] private GameObject settingCanvas,keyLayoutCanvas;
    [SerializeField] private Panel panel;
    [SerializeField] private ClosePanel closePanel;

    [SerializeField] private TextMeshProUGUI bgmText,seText;

    [SerializeField] private AudioMixer audioMixer;
    

    [SerializeField] private Slider bgmSlider, seSlider;
    public float _bgmVolume, _seVolume;
    private bool _isShown;
    
    private OptionDataContainer _optionDataContainer;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => OptionDataContainer.Inst.soundSettingsData != null);

        _optionDataContainer = OptionDataContainer.Inst;

        bgmSlider.value = _optionDataContainer.soundSettingsData.musicVolume;
        seSlider.value = _optionDataContainer.soundSettingsData.sfxVolume;

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
        _optionDataContainer.soundSettingsData.musicVolume = bgmSlider.value;
        _bgmVolume = Mathf.FloorToInt(bgmSlider.value * 100);
        bgmText.text = $"- {_bgmVolume} % -";
        ApplySoundSettings();
    }

    public void ChangeSFXVolume()
    {
        _optionDataContainer.soundSettingsData.sfxVolume = seSlider.value;
        _seVolume = Mathf.FloorToInt(seSlider.value * 100);
        seText.text = $"- {_seVolume} % -";
        ApplySoundSettings();
    }
    
    private void ApplySoundSettings()
    {
        var musicVolume = _optionDataContainer.soundSettingsData.musicVolume;
        var musicMixerValue = musicVolume > 0.001f ? Mathf.Log10(musicVolume) * 20 : -80f;
        audioMixer.SetFloat("BGM", musicMixerValue);
        
        var sfxVolume = _optionDataContainer.soundSettingsData.sfxVolume;
        var sfxMixerValue = sfxVolume > 0.001f ? Mathf.Log10(sfxVolume) * 20 : -80f;
        audioMixer.SetFloat("SFX", sfxMixerValue);
        
        _optionDataContainer.Save();
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
