using System;
using System.Collections.Generic;
using Core.Controllers;
using Cysharp.Threading.Tasks;
using SandyCore;
using UnityEngine;

namespace Core.Domain.Units
{
    public class AIController : MonoBehaviour
    {
        [Header("기본 설정")]
        [SerializeField] private Unit unit;
        [SerializeField] private int moveThreshold = 2;
        [SerializeField] private int actionThreshold = 2;
        
        [Header("디버깅")]
        [SerializeField] private bool debugPath = false;
        
        // 경로 관련 변수
        private List<Vector2Int> _currentPath;
        private int _currentPathIndex;
        private Vector2Int _lastTargetPosition;
        
        // 상태 관련 변수
        private int _moveWaitingTurns;
        private int _actionWaitingTurns;
        private bool _isAttackReady;
        private bool _isMoving;
        
        // 캐시된 참조
        private GameplayController _gameplayController;
        private GridController _gridController;
        
        private void Awake()
        {
            _gameplayController = ApplicationManager.Instance.GetModule<GameplayController>();
            _gridController = _gameplayController.GridController;
        }
        
        private void OnEnable()
        {
            ApplicationManager.Instance.GetModule<TurnController>().OnTurnChanged += HandleTurnChanged;
        }

        private void OnDisable()
        {
            ApplicationManager.Instance.GetModule<TurnController>().OnTurnChanged -= HandleTurnChanged;
        }

        private async void HandleTurnChanged(int turnCount)
        {
            // 이미 이동 중이면 스킵
            if (_isMoving)
                return;

            // 공격 준비 상태면 공격 실행
            if (_isAttackReady)
            {
                ExecuteAttack();
                return;
            }
            
            _actionWaitingTurns++;
            _moveWaitingTurns++;

            // 플레이어와의 거리 확인
            var currentPosition = _gridController.GetGridPosition(transform.position);
            var playerPosition = PlayerController.Player.GridPosition;

            // 인접한 경우 공격 준비
            if (_actionWaitingTurns > actionThreshold)
            {
                if (IsAdjacentToPlayer(currentPosition, playerPosition))
                {
                    _actionWaitingTurns = 0;
                    
                    var dir = playerPosition - currentPosition;
                    unit.Turn(dir);

                    PrepareAttack();
                    return;
                }
            }

            // 이동 대기 턴 체크
            if (_moveWaitingTurns > moveThreshold)
            {
                _moveWaitingTurns = 0;
                await MoveTowardsPlayer(currentPosition, playerPosition);
            }
        }
        
        /// <summary>
        /// 플레이어와 인접했는지 확인
        /// </summary>
        private bool IsAdjacentToPlayer(Vector2Int currentPos, Vector2Int playerPos)
        {
            var distance = playerPos - currentPos;
            return (Math.Abs(distance.x) == 1 && distance.y == 0) || 
                   (Math.Abs(distance.y) == 1 && distance.x == 0);
        }
        
        /// <summary>
        /// 공격 준비
        /// </summary>
        private void PrepareAttack()
        {
            _isAttackReady = true;
            unit.SetAttackReady(true);
            
            // 경로 초기화
            ClearPath();
        }
        
        /// <summary>
        /// 공격 실행
        /// </summary>
        private void ExecuteAttack()
        {
            unit.SetAttackReady(false);
            unit.ExecuteAttack();
            _isAttackReady = false;
        }
        
        /// <summary>
        /// 플레이어를 향해 이동
        /// </summary>
        private async UniTask MoveTowardsPlayer(Vector2Int currentPos, Vector2Int targetPos)
        {
            if (IsAdjacentToPlayer(currentPos, targetPos))
                return;
            
            CalculatePath(currentPos, targetPos);
            
            // 유효한 경로가 있으면 다음 위치로 이동
            if (HasValidPath())
            {
                await MoveAlongPath(currentPos);
            }
            /*else
            {
                await AttemptFallbackMove(currentPos, targetPos);
            }*/
        }
        
        /// <summary>
        /// 경로 재계산이 필요한지 확인
        /// </summary>
        private bool NeedsPathRecalculation(Vector2Int playerPos)
        {
            // 경로가 없거나 목표 위치가 변경된 경우
            if (_currentPath == null || _currentPath.Count == 0) return true;
            if (_lastTargetPosition != playerPos) return true;
            
            // 현재 경로 인덱스가 범위를 벗어난 경우
            if (_currentPathIndex >= _currentPath.Count) return true;
            
            // 다음 위치가 막힌 경우
            if (_currentPathIndex < _currentPath.Count)
            {
                var nextPos = _currentPath[_currentPathIndex];
                if (_gridController.IsOccupied(nextPos)) return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// 플레이어까지의 경로 계산
        /// </summary>
        private void CalculatePath(Vector2Int startPos, Vector2Int playerPos)
        {
            ClearPath();
            
            var targetPositions = GetPlayerAdjacentCells(playerPos);
            if (targetPositions.Count == 0) return;
            
            Vector2Int[] bestPath = null;
            int shortestDistance = int.MaxValue;
            
            foreach (var target in targetPositions)
            {
                var path = _gridController.PathFinder.FindPath(startPos, target);
                if (path != null && path.Length > 0 && path.Length < shortestDistance)
                {
                    shortestDistance = path.Length;
                    bestPath = path;
                }
            }
            
            // 최적 경로 설정
            if (bestPath != null)
            {
                _currentPath = new List<Vector2Int>(bestPath);
                _currentPathIndex = 0;
                _lastTargetPosition = playerPos;
            }
        }
        
        /// <summary>
        /// 플레이어 인접 셀 목록 반환 (이동 가능한 셀만)
        /// </summary>
        private List<Vector2Int> GetPlayerAdjacentCells(Vector2Int playerPos)
        {
            var directions = new[]
            {
                Vector2Int.up, Vector2Int.down, 
                Vector2Int.right, Vector2Int.left
            };
            
            var validCells = new List<Vector2Int>();
            
            foreach (var dir in directions)
            {
                var cell = playerPos + dir;
                if (!_gridController.IsOccupied(cell))
                {
                    validCells.Add(cell);
                }
            }
            
            return validCells;
        }
        
        /// <summary>
        /// 유효한 경로가 있는지 확인
        /// </summary>
        private bool HasValidPath()
        {
            return _currentPath != null && 
                   _currentPath.Count > 0 && 
                   _currentPathIndex < _currentPath.Count;
        }
        
        /// <summary>
        /// 경로를 따라 이동
        /// </summary>
        private async UniTask MoveAlongPath(Vector2Int currentPos)
        {
            if (!HasValidPath()) return;
            
            var nextPosition = _currentPath[_currentPathIndex];
            
            // 다음 위치가 현재 위치와 같으면 인덱스만 증가
            if (nextPosition == currentPos)
            {
                _currentPathIndex++;
                if (_currentPathIndex < _currentPath.Count)
                {
                    nextPosition = _currentPath[_currentPathIndex];
                }
                else
                {
                    return;
                }
            }
            
            // 이동 가능 여부 확인
            if (!_gridController.IsOccupied(nextPosition))
            {
                var dir = nextPosition - currentPos;
                unit.Turn(dir);
                
                _isMoving = true;
                await unit.MoveAsync(nextPosition);
                _isMoving = false;
                
                _currentPathIndex++;
                
                if (debugPath)
                {
                    Debug.Log($"[AI] 이동 완료: {nextPosition}, 남은 경로: {_currentPath.Count - _currentPathIndex}");
                }
            }
            else
            {
                // 경로가 막혔으면 재계산 필요
                if (debugPath)
                {
                    Debug.Log($"[AI] 경로 막힘 감지: {nextPosition}");
                }
                ClearPath();
            }
        }
        
        /// <summary>
        /// 경로를 찾을 수 없을 때 대체 이동 시도
        /// </summary>
        private async UniTask AttemptFallbackMove(Vector2Int currentPos, Vector2Int playerPos)
        {
            if (debugPath)
            {
                Debug.Log("[AI] 대체 이동 모드 활성화");
            }
            
            // 플레이어 방향으로 직접 이동 시도
            var direction = playerPos - currentPos;
            var moveOptions = new List<Vector2Int>();
            
            // 우선순위: X축 이동, Y축 이동
            if (direction.x != 0)
            {
                moveOptions.Add(new Vector2Int(Math.Sign(direction.x), 0));
            }
            if (direction.y != 0)
            {
                moveOptions.Add(new Vector2Int(0, Math.Sign(direction.y)));
            }
            
            // 대각선 방향도 고려 (장애물 회피용)
            if (direction.x != 0 && direction.y != 0)
            {
                moveOptions.Add(new Vector2Int(Math.Sign(direction.x), Math.Sign(direction.y)));
            }
            
            // 이동 시도
            foreach (var move in moveOptions)
            {
                unit.Turn(move);
                
                var targetPos = currentPos + move;
                if (!_gridController.IsOccupied(targetPos))
                {
                    _isMoving = true;
                    await unit.MoveAsync(targetPos);
                    _isMoving = false;
                    return;
                }
            }
            
            // 모든 방향이 막혔으면 랜덤 이동 시도
            await AttemptRandomMove(currentPos);
        }
        
        /// <summary>
        /// 랜덤 방향으로 이동 시도 (막힌 상황 탈출용)
        /// </summary>
        private async UniTask AttemptRandomMove(Vector2Int currentPos)
        {
            var directions = new[]
            {
                Vector2Int.up, Vector2Int.down,
                Vector2Int.right, Vector2Int.left
            };
            
            // 방향 섞기
            for (int i = directions.Length - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (directions[i], directions[j]) = (directions[j], directions[i]);
            }
            
            foreach (var dir in directions)
            {
                var targetPos = currentPos + dir;
                if (!_gridController.IsOccupied(targetPos))
                {
                    _isMoving = true;
                    await unit.MoveAsync(targetPos);
                    _isMoving = false;
                    
                    if (debugPath)
                    {
                        Debug.Log($"[AI] 랜덤 이동: {targetPos}");
                    }
                    return;
                }
            }
            
            if (debugPath)
            {
                Debug.Log("[AI] 이동 불가 - 모든 방향 막힘");
            }
        }
        
        /// <summary>
        /// 경로 초기화
        /// </summary>
        private void ClearPath()
        {
            _currentPath?.Clear();
            _currentPath = null;
            _currentPathIndex = 0;
            _lastTargetPosition = Vector2Int.zero;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!debugPath || _currentPath == null || _gridController == null) return;
            
            Gizmos.color = Color.yellow;
            for (int i = _currentPathIndex - 1; i < _currentPath.Count - 1; i++)
            {
                var start = _gridController.GetWorldPosition(_currentPath[i]) + new Vector3(0, 0.5f);
                var end = _gridController.GetWorldPosition(_currentPath[i + 1]) + new Vector3(0, 0.5f);
                Gizmos.DrawLine(start, end);
            }
        }
#endif
    }
}