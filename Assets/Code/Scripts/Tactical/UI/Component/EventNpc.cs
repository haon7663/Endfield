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
    [SerializeField] [TextArea] private string speakLine;
    [FormerlySerializedAs("txt")] [SerializeField] private TextMeshProUGUI speakLineTxt; 
    [SerializeField] private Panel speakLinePanel;
    [SerializeField] private Panel giveItemPanel;
    [FormerlySerializedAs("imagss")] public Image icon;
    [SerializeField] private List<int> interactTileIndex = new List<int>();

    private bool _isActive = false;

    private void Start()
    {
        speakLineTxt.text = speakLine;
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

    public void ResultSprite(Sprite spr)
    {
        Hide(speakLinePanel);
        Show(giveItemPanel);
        icon.sprite = spr;
    }



    private void Show(Panel panel)
    {
        panel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
        _isActive = true;
    }

    private void Hide(Panel panel)
    {
        panel.SetPosition(PanelStates.Hide, true, 0.3f, Ease.OutBack);
        _isActive = false;
    }
    
    
}
