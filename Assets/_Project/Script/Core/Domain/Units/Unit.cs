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
        
        public float CurrentHealth { get; private set; }
        
        public Vector2Int GridPosition { get; private set; }
        public Vector2Int Direction { get; private set; }
        
        private static readonly int IsFrontDash = Animator.StringToHash("isFrontDash");
        private static readonly int IsBackDash = Animator.StringToHash("isBackDash");
        private static readonly int IsReady = Animator.StringToHash("isReady");
        private static readonly int Attack = Animator.StringToHash("attack");

        public override void OnSpawn()
        {
            base.OnSpawn();
            
            var unitController = ApplicationManager.Instance.GetModule<UnitController>();
            unitController.Register(gameObject.GetInstanceID(), this);

            CurrentHealth = 5;
            
            var gridController = ApplicationManager.Instance.GetModule<GameplayController>().GridController;
            var gridPos = gridController.GetGridPosition(transform.position);
            
            gridController.OccupyGrid(gridPos, gameObject.GetInstanceID());
            GridPosition = gridPos;
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            
            var unitController = ApplicationManager.Instance.GetModule<UnitController>();
            unitController.UnRegister(gameObject.GetInstanceID());
            
            var gridController = ApplicationManager.Instance.GetModule<GameplayController>().GridController;
            gridController.ReleaseGrid(GridPosition);
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

            var sign = Mathf.Sign(targetPos.x - startPos.x);
            var dirSign = Mathf.Sign(Direction.x);
            
            var moveKey = Mathf.Approximately(sign, dirSign) ? IsFrontDash : IsBackDash;
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
        
        public void Turn(Vector2Int dir)
        {
            Direction = dir;
            
            if (dir.x != 0)
                transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, transform.localScale.y, transform.localScale.z);
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                ApplicationManager.Instance.PoolingService.ReturnPoolable(this);
            }
        }

        public void SetAttackReady(bool value)
        {
            animator.SetBool(IsReady, value);
        }

        public void ExecuteAttack()
        {
            animator.SetTrigger(Attack);
            
            var targetGridPos = GridPosition + Direction;
            
            var gridController = ApplicationManager.Instance.GetModule<GameplayController>().GridController;
            var targetObjectId = gridController.GetOccupiedObjectId(targetGridPos);

            if (targetObjectId != -1)
            {
                var unitController = ApplicationManager.Instance.GetModule<UnitController>();
                if (unitController.TryGet<Unit>(targetObjectId, out var target))
                {
                    target.TakeDamage(1);
                }
            }
        }
    }
}