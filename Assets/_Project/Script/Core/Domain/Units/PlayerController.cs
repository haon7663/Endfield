using Core.Controllers;
using Cysharp.Threading.Tasks;
using SandyCore;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Domain.Units
{
    public class PlayerController : MonoBehaviour
    {
        public static Unit Player;
        
        [SerializeField] private Unit unit;
        
        private void Awake()
        {
            Player = unit;
            
            var inputSystemActions = new InputSystem_Actions();
            inputSystemActions.Player.Enable();

            inputSystemActions.Player.MoveLeft.performed += HandleMoveLeft;
            inputSystemActions.Player.MoveRight.performed += HandleMoveRight;
            inputSystemActions.Player.MoveUp.performed += HandleMoveUp;
            inputSystemActions.Player.MoveDown.performed += HandleMoveDown;
            inputSystemActions.Player.TurnLeft.performed += HandleTurnLeft;
            inputSystemActions.Player.TurnRight.performed += HandleTurnRight;
            inputSystemActions.Player.TurnUp.performed += HandleTurnUp;
            inputSystemActions.Player.TurnDown.performed += HandleTurnDown;
        }

        #region Input System Actions
        
        private void HandleMoveLeft(InputAction.CallbackContext ctx)
        {
            MoveToDirection(Vector2Int.left);
        }
        
        private void HandleMoveRight(InputAction.CallbackContext ctx)
        {
            MoveToDirection(Vector2Int.right);
        }
        
        private void HandleMoveUp(InputAction.CallbackContext ctx)
        {
            MoveToDirection(Vector2Int.up);
        }
        
        private void HandleMoveDown(InputAction.CallbackContext ctx)
        {
            MoveToDirection(Vector2Int.down);
        }

        private void HandleTurnLeft(InputAction.CallbackContext ctx)
        {
            TurnToDirection(Vector2Int.left);
        }

        private void HandleTurnRight(InputAction.CallbackContext ctx)
        {
            TurnToDirection(Vector2Int.right);
        }

        private void HandleTurnUp(InputAction.CallbackContext ctx)
        {
            TurnToDirection(Vector2Int.up);
        }

        private void HandleTurnDown(InputAction.CallbackContext ctx)
        {
            TurnToDirection(Vector2Int.down);
        }
        
        #endregion

        private void MoveToDirection(Vector2Int direction)
        {
            var gridController = ApplicationManager.Instance.GetModule<GameplayController>().GridController;
            
            var targetPos = unit.GridPosition + direction;

            if (!gridController.IsValidGridPosition(targetPos))
                return;
            
            unit.MoveAsync(targetPos).Forget();

            ChangeTurn();
        }

        private void TurnToDirection(Vector2Int direction)
        {
            unit.Turn(direction);
            unit.ExecuteAttack();
            
            ChangeTurn();
        }

        private void ChangeTurn()
        {
            ApplicationManager.Instance.GetModule<TurnController>().ForceChangeTurn();
        }
    }
}