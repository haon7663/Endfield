using Core.Controllers;
using Cysharp.Threading.Tasks;
using SandyCore;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Domain.Units
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;
        public static Unit Player;
        
        [SerializeField] private Unit unit;
        
        private void Awake()
        {
            Instance = this;
            Player = unit;
            
            var inputSystemActions = new InputSystem_Actions();
            inputSystemActions.Player.Enable();

            inputSystemActions.Player.MoveLeft.performed += HandleMoveLeft;
            inputSystemActions.Player.MoveRight.performed += HandleMoveRight;
            inputSystemActions.Player.MoveUp.performed += HandleMoveUp;
            inputSystemActions.Player.MoveDown.performed += HandleMoveDown;
        }

        private void HandleMoveLeft(InputAction.CallbackContext ctx)
        {
            var gridController = ApplicationManager.Instance.GetModule<GameplayController>().GridController;
            
            var targetPos = unit.GridPosition + new Vector2Int(-1, 0);

            if (!gridController.IsValidGridPosition(targetPos))
                return;
            
            unit.MoveAsync(targetPos).Forget();
            
            ChangeTurn();
        }
        
        private void HandleMoveRight(InputAction.CallbackContext ctx)
        {
            var gridController = ApplicationManager.Instance.GetModule<GameplayController>().GridController;
            
            var targetPos = unit.GridPosition + new Vector2Int(1, 0);

            if (!gridController.IsValidGridPosition(targetPos))
                return;
            
            unit.MoveAsync(targetPos).Forget();
            
            ChangeTurn();
        }
        
        private void HandleMoveUp(InputAction.CallbackContext ctx)
        {
            var gridController = ApplicationManager.Instance.GetModule<GameplayController>().GridController;
            
            var targetPos = unit.GridPosition + new Vector2Int(0, 1);

            if (!gridController.IsValidGridPosition(targetPos))
                return;
            
            unit.MoveAsync(targetPos).Forget();
            
            ChangeTurn();
        }
        
        private void HandleMoveDown(InputAction.CallbackContext ctx)
        {
            var gridController = ApplicationManager.Instance.GetModule<GameplayController>().GridController;
            
            var targetPos = unit.GridPosition + new Vector2Int(0, -1);

            if (!gridController.IsValidGridPosition(targetPos))
                return;
            
            unit.MoveAsync(targetPos).Forget();

            ChangeTurn();
        }

        private void ChangeTurn()
        {
            ApplicationManager.Instance.GetModule<TurnController>().ForceChangeTurn();
        }
    }
}