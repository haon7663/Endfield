using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EventNpc : MonoBehaviour
{
    [SerializeField] [TextArea] private string speakLine,failLine;
    [FormerlySerializedAs("txt")] [SerializeField] private TextMeshProUGUI speakLineTxt;
    [SerializeField] private TextMeshProUGUI giftNameTxt,failTxt;
    public List<Panel> panels = new List<Panel>();
    [SerializeField] private Panel speakLinePanel;
    [SerializeField] private Panel giveItemPanel;
    [SerializeField] private Panel failLinePanel;
    [FormerlySerializedAs("imagss")] public Image icon;
   
    [SerializeField] private List<int> interactTileIndex = new List<int>();


    private bool _isActive = false;

    private void Start()
    {
        speakLineTxt.text = speakLine;
        failTxt.text = failLine;
    }
    
    private void Update()
    {
        if(!_isActive)
            foreach (var index in interactTileIndex)
            {
                if(GridManager.Inst.GetTile(index).content) Show(speakLinePanel);
            }


        if (_isActive)
        {
            bool _onPlayer = false;
            foreach (var index in interactTileIndex)
            {
                if (GridManager.Inst.GetTile(index).content) _onPlayer = true;
            }

            if (!_onPlayer) Hide(speakLinePanel);
        }
      
    }

    public void ResultSprite(Sprite spr,string iconName)
    {
        Hide(speakLinePanel);
        Show(giveItemPanel);
        icon.sprite = spr;
        giftNameTxt.text = iconName;
    }

    public void GambleFail()
    {
        Show(failLinePanel);
    }



    private void Show(Panel panel)
    {
        foreach(Panel _panel in panels)
        {
            if (panel == _panel)
                panel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
            else
                Hide(_panel);
        }

        if (panel == speakLinePanel)
        {
            _isActive = true;
        }
    }

    public void Hide(Panel panel)
    {
        panel.SetPosition(PanelStates.Hide, true, 0.3f, Ease.OutBack);
        if (panel == speakLinePanel)
        {
            _isActive = false;
        }
    }

    public void HideAllPanels()
    {
        foreach (var panel in panels)
        {
            panel.SetPosition(PanelStates.Hide, true, 0.3f, Ease.OutBack);
        }
    }
    
    
}
