using System;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
   [SerializeField] private int chestPrice,victoryPercent;
   [SerializeField] private TextMeshProUGUI percentTxt, priceTxt;
   [SerializeField] private Panel panel;
   [SerializeField] private List<int> interactTileIndex = new List<int>();
   [SerializeField] private KeyCode keyCode;
   
   private List<int> _percentIndex = new List<int>();
 
   private bool _isActive = false;

   private void Awake()
   {
      priceTxt.text = chestPrice.ToString() + " ml";
      percentTxt.text = "성공 확률 " + victoryPercent.ToString() + "%";
      
      for (int i = 1; i < victoryPercent + 1; i++) _percentIndex.Add(i);
   }

   public void Gamble()
   {
      if(DataManager.Inst.Data.gold - chestPrice<0) return;
      GoldController.Inst.ReCountGold(DataManager.Inst.Data.gold,DataManager.Inst.Data.gold-=chestPrice);
      int random = Random.Range(1, 100 + 1);
      (_percentIndex.Contains(random) ?(Action) Win : Lose)();

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
         if (Input.GetKeyDown(keyCode)) Gamble();

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

   public void Win()
   {
      Debug.Log("성공!");
   }

   public void Lose()
   {
      Debug.Log("실패!");
   }
}