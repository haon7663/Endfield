using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

using System.Linq;
using Newtonsoft.Json;
using UnityEngine.UI;

public class EventNpc : MonoBehaviour
{
    [SerializeField] [TextArea] private string speakLine;
    [SerializeField] private TextMeshProUGUI txt;
    [SerializeField] private Panel panel;
    public Image imagss;
    [SerializeField] private List<int> interactTileIndex = new List<int>();

    private bool _isActive = false;

    private void Start()
    {
        txt.text = speakLine;
        GetSkillSprite("a");
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

    public void Sprite(Sprite spr)
    {
        imagss.sprite = spr;
    }

    public void GetSkillSprite(string path)
    {
        if (!path.StartsWith("Icon/"))
            path = "Icon/" + path;

        var sprite = Resources.Load<Sprite>(path);
        Sprite(sprite);
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
