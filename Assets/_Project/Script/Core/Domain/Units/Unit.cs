using Core.Controllers;
using Cysharp.Threading.Tasks;
using SandyCore;
using SandyToolkitCore.Pooling;
using UnityEngine;

namespace Core.Domain.Units
{
    public class Unit : PoolingBase
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float moveSpeed;
        
        public Vector2Int GridPosition { get; private set; }
        public Vector2Int Direction { get; private set; }
        
        private static readonly int _isFrontDash = Animator.StringToHash("isFrontDash");
        private static readonly int _isBackDash = Animator.StringToHash("isBackDash");
        private static readonly int IsReady = Animator.StringToHash("isReady");
        private static readonly int Attack = Animator.StringToHash("attack");

        public override void OnSpawn()
        {
            base.OnSpawn();
            
            var gridController = ApplicationManager.Instance.GetModule<GameplayController>().GridController;
            var gridPos = gridController.GetGridPosition(transform.position);
            
            gridController.OccupyGrid(gridPos, gameObject.GetInstanceID());
            GridPosition = gridPos;
        }
        
        public async UniTask MoveAsync(Vector2Int gridPos)
        {
            var gridController = ApplicationManager.Instance.GetModule<GameplayController>().GridController;

            if (gridController.IsOccupied(gridPos))
                return;
            
            gridController.ReleaseGrid(GridPosition);
            GridPosition = gridPos;
            gridController.OccupyGrid(GridPosition, gameObject.GetInstanceID());
            
            var startPos = transform.position;
            var targetPos = ApplicationManager.Instance.GetModule<GameplayController>().GridController.GetWorldPosition(gridPos) + new Vector3(0, 0.5f);
            
            var moveKey = (targetPos.x - startPos.x) > 0 ? _isFrontDash : _isBackDash;
            animator.SetBool(moveKey, true);

            try
            {
                var progress = 0f;
                while (progress < 1f)
                {
                    transform.position = Vector3.Lerp(startPos, targetPos, progress);
                    progress += Time.deltaTime * moveSpeed;

                    await UniTask.Yield(timing: PlayerLoopTiming.Update);
                }
            }
            finally
            {
                animator.SetBool(moveKey, false);
            }
        }

        public void SetAttackReady(bool value)
        {
            animator.SetBool(IsReady, value);
        }

        public void ExecuteAttack()
        {
            animator.SetTrigger(Attack);
        }
    }
}