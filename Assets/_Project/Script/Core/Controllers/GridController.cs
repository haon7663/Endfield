using System.Collections.Generic;
using SandyCore;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using Tile = Core.Views.Map.Tile;

namespace Core.Controllers
{
    public class GridController
    {
        public const float TILE_HORIZONTAL_INTERVAL = 1.2f;
        public const float TILE_VERTICAL_INTERVAL = 3.5f;

        public Vector2Int[] GridPositions { get; private set; }
        public Vector2Int GridSize { get; private set; }
        public Vector2 GridOffset { get; private set; }
        
        private readonly Dictionary<int, int> _occupiedGrid = new();

        public void Initialize(Vector2Int gridSize)
        {
            GridSize = gridSize;
            GridPositions = new Vector2Int[gridSize.x * gridSize.y];
            GridOffset = (new Vector2(gridSize.x, gridSize.y) * 0.5f) - new Vector2(0.5f, 0.5f);
            
            GenerateGrid(gridSize);
        }

        private void GenerateGrid(Vector2Int gridSize)
        {
            for (var x = 0; x < gridSize.x; x++)
            {
                for (var y = 0; y < gridSize.y; y++)
                {
                    var tilePos = new Vector2Int(x, y);
                    var worldPos = GetWorldPosition(tilePos);
                    
                    var tile = ApplicationManager.Instance.PoolingService.GetPoolable<Tile>("Tile", worldPos);
                    
                    var index = GetIndex(x, y);
                    GridPositions[index] = tilePos;
                }
            }
        }

        public Vector3 GetWorldPosition(Vector2Int gridPosition)
        {
            return new Vector3(
                (gridPosition.x - GridOffset.x) * TILE_HORIZONTAL_INTERVAL,
                0f,
                (gridPosition.y - GridOffset.y) * TILE_VERTICAL_INTERVAL
            );
        }
        
        public Vector2Int GetGridPosition(Vector3 worldPosition)
        {
            var x = Mathf.RoundToInt(worldPosition.x / TILE_HORIZONTAL_INTERVAL + GridOffset.x);
            var y = Mathf.RoundToInt(worldPosition.z / TILE_VERTICAL_INTERVAL + GridOffset.y);
            
            x = Mathf.Clamp(x, 0, GridSize.x - 1);
            y = Mathf.Clamp(y, 0, GridSize.y - 1);

            return new Vector2Int(x, y);
        }
        
        private int GetIndex(int x, int y)
        {
            return y * GridSize.x + x;
        }
        
        public bool IsValidGridPosition(Vector2Int gridPosition)
        {
            return IsValidGridPosition(gridPosition.x, gridPosition.y);
        }

        private bool IsValidGridPosition(int x, int y)
        {
            return x >= 0 && x < GridSize.x && 
                   y >= 0 && y < GridSize.y;
        }

        #region Occupy

        public bool OccupyGrid(Vector2Int gridPosition, int objectId)
        {
            if (!IsValidGridPosition(gridPosition))
                return false;
    
            var index = GetIndex(gridPosition.x, gridPosition.y);
            if (_occupiedGrid.ContainsKey(index))
                return false;
    
            _occupiedGrid[index] = objectId;
            return true;
        }
        
        public bool ReleaseGrid(Vector2Int gridPosition)
        {
            if (!IsValidGridPosition(gridPosition))
                return false;
    
            var index = GetIndex(gridPosition.x, gridPosition.y);
            return _occupiedGrid.Remove(index);
        }
        
        public bool IsOccupied(Vector2Int gridPosition)
        {
            if (!IsValidGridPosition(gridPosition))
                return true;
    
            var index = GetIndex(gridPosition.x, gridPosition.y);
            return _occupiedGrid.ContainsKey(index);
        }

        public int GetOccupiedObjectId(Vector2Int gridPosition)
        {
            if (!IsValidGridPosition(gridPosition))
                return -1;
    
            var index = GetIndex(gridPosition.x, gridPosition.y);
            return _occupiedGrid.GetValueOrDefault(index, -1);
        }
        
        public void ClearAllOccupied()
        {
            _occupiedGrid.Clear();
        }

        #endregion
    }
}