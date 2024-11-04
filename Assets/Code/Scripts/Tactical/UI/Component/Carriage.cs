using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Carriage : MonoBehaviour
{
    [SerializeField] private List<int> interactTileIndex = new List<int>();
    [SerializeField] private Panel panel;
    [SerializeField] private KeyCode keyCode;
    [SerializeField] private ShopController shopController;
    private bool _isActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Update()
    {
        if(!_isActive)
            foreach (var index in interactTileIndex)
            {
                if(GridManager.Inst.GetTile(index).content) Show();
            }


        if (_isActive)
        {
            if (Input.GetKeyDown(keyCode)&& !shopController.IsShopActive()) shopController.Show();

            bool _onPlayer = false;
            foreach (var index in interactTileIndex)
            {
                if (GridManager.Inst.GetTile(index).content) _onPlayer = true;
            }

            if (!_onPlayer) Hide();
        }
      
    }

    public void Show()
    {
        panel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
        _isActive = true;
    }

    public void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true, 0.3f, Ease.OutBack);
        _isActive = false;
    }

}
