using UnityEngine;

public class UIManager : Singleton<UIManager>
{
   [SerializeField]private bool isAnyUiOpen = false;

   public void UIShow(bool show)
   {
      if (show)
      {
         isAnyUiOpen = true;
         GameManager.Inst.isGameActive = false;
         return;
      }
      else
      {
         isAnyUiOpen = false;
         GameManager.Inst.isGameActive = true;
      }
   }



   public bool AlreadyUIOpen()
   {
      return isAnyUiOpen;
   }
}
