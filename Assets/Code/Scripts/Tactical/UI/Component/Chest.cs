using System;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
   private EventController _eventController;
   [SerializeField] private int chestPrice,victoryPercent;
   [SerializeField] private TextMeshProUGUI percentTxt, priceTxt;
   [SerializeField] private Panel panel;
   [SerializeField] private List<int> interactTileIndex = new List<int>();
   [SerializeField] private KeyCode keyCode;

    [SerializeField] private List<ChestEvent> events = new List<ChestEvent>();
   
   private List<int> _percentIndex = new List<int>();
 
   private bool _isActive = false;
   [SerializeField] private bool _isOpen = false;
    private Animator _animator;

    private void Awake()
   {
        _animator = GetComponent<Animator>();
        if (transform.parent.parent.TryGetComponent(out EventController eventController))
         {
            _eventController = transform.parent.GetComponentInParent<EventController>();
         }
        
        GetEvent();
        priceTxt.text = chestPrice.ToString() + " ml";
        percentTxt.text = "성공 확률 " + victoryPercent.ToString() + "%";
      
        for (int i = 1; i < victoryPercent + 1; i++) _percentIndex.Add(i);


   }

    private void GetEvent()
    {
        var chestEvents = new List<ChestEvent>
        { 
            new GetGold_ChestEvent(),
            new GetSkill_ChestEvent(),
          // new GetSkillUpgrade_ChestEvent(),
            new GetHp_ChestEvent(),
        };
        events.AddRange(chestEvents);      
    }

   public void Gamble()
   {
      if(DataManager.Inst.Data.gold - chestPrice<0 && !_isOpen) return;
      GoldController.Inst.ReCountGold(-chestPrice);
      int random = Random.Range(1, 100 + 1);
      (_percentIndex.Contains(random) ?(Action) Win : Lose)();

   }

   private void Update()
   {
      if(!_isActive && !_isOpen)
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
        _isOpen = true;
        _animator.Play("Chest_Open");
        Hide();
        int random = Random.Range(0, events.Count);
        var (sprite,name) = events[random].Excute();
        _eventController.GambleWin(sprite,name);
        Debug.Log("성공!");
   }

   public void Lose()
   {
        _eventController.GambleFail();
   }
}
