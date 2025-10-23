using System;
using Core.Controllers;
using Cysharp.Threading.Tasks;
using SandyCore;
using UnityEngine;

namespace Core.Domain.Units
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        [SerializeField] private int moveThreshold;

        private int _moveWaiting;
        private bool _attackReady;
        
        private void OnEnable()
        {
            ApplicationManager.Instance.GetModule<TurnController>().OnTurnChanged += HandleTurnChanged;
        }

        private void HandleTurnChanged(int turnCount)
        {
            if (_attackReady)
            {
                
                
                return;
            }
            
            var gridController = ApplicationManager.Instance.GetModule<GameplayController>().GridController;
            var player = PlayerController.Player;
            
            var startGridPos = gridController.GetGridPosition(transform.position);

            var xDistance = player.GridPosition.x - startGridPos.x;
            var yDistance = player.GridPosition.y - startGridPos.y;
            
            var xDistanceAbs = Mathf.Abs(xDistance);
            var yDistanceAbs = Mathf.Abs(yDistance);
            
            if ((xDistanceAbs <= 1 && yDistanceAbs == 0) || (yDistanceAbs <= 1 && xDistanceAbs == 0))
            {
                _attackReady = true;
            }
            else if (++_moveWaiting > moveThreshold)
            {
                _moveWaiting = 0;

                var moveDirection = Vector2Int.zero;
                
                if (xDistanceAbs > 0)
                {
                    moveDirection = xDistance > 0 ? new Vector2Int(1, 0) : new Vector2Int(-1, 0);
                }

                if (yDistanceAbs > 0)
                {
                    moveDirection = yDistance > 0 ? new Vector2Int(0, 1) : new Vector2Int(0, -1);
                }

                var targetGridPos = startGridPos + moveDirection;

                unit.MoveAsync(targetGridPos).Forget();
            }
        }
        
        private void OnDisable()
        {
            ApplicationManager.Instance.GetModule<TurnController>().OnTurnChanged -= HandleTurnChanged;
        }
    }
}
