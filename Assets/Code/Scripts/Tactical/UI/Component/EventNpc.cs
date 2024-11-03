using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EventNpc : MonoBehaviour
{
    [SerializeField] [TextArea] private string speakLine;
    [SerializeField] private TextMeshProUGUI txt;
    [SerializeField] private Panel panel;
    [SerializeField] private List<int> interactTileIndex = new List<int>();
    private bool _isActive = false;

    private void Start()
    {
        txt.text = speakLine;
    }
    
    private void Update()
    {
        if(!_isActive)
            foreach (var index in interactTileIndex)
            {
                if(GridManager.Inst.GetTile(index).content) Show();
            }


        if (_isActive)
        {
            bool _onPlayer = false;
            foreach (var index in interactTileIndex)
            {
                if (GridManager.Inst.GetTile(index).content) _onPlayer = true;
            }

            if (!_onPlayer) Hide();
        }
      
    }
    
    private void Show()
    {
        panel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
        _isActive = true;
    }

    private void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true, 0.3f, Ease.OutBack);
        _isActive = false;
    }
    
    
}
