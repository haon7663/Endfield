using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Core.Controllers
{
    public readonly struct PathFinderNode : IEquatable<PathFinderNode>
    {
        public Vector2Int Cell { get; }
        public Vector2Int ParentNodeCell { get; }
        
        public int G { get; }
        public int H { get; }
        public int F => G + H;

        public PathFinderNode(Vector2Int cell, int g, int h, Vector2Int parentNodeCell)
        {
            Cell = cell;
            G = g;
            H = h;
            ParentNodeCell = parentNodeCell;
        }

        #region [Equality]
        public bool Equals(PathFinderNode other)
        {
            return Cell.Equals(other.Cell);
        }

        public override bool Equals(object obj)
        {
            return obj is PathFinderNode other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Cell.GetHashCode();
        }

        public static bool operator ==(PathFinderNode left, PathFinderNode right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PathFinderNode left, PathFinderNode right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
    
    public class PathFinder
    {
        private readonly Dictionary<Vector2Int, PathFinderNode> _pathFinderNodes = new();
        private readonly PriorityQueue<PathFinderNode> _pathFinderQueue = new(100, new ComparePathFinderNodeByFValue());
        private readonly HashSet<Vector2Int> _visitedCells = new();

        private readonly GridController _gridController;

        public PathFinder(GridController gridController)
        {
            _gridController = gridController;
        }
        
        public Vector2Int[] FindPath(Vector2Int start, Vector2Int end)
        {
            if (start.Equals(end))
                return new[] { start };

            _pathFinderNodes.Clear();
            _pathFinderQueue.Clear();
            _visitedCells.Clear();

            var h = CalculateHeuristic(start, end);
            var startNode = new PathFinderNode(start, g: 0, h: h, parentNodeCell: start);

            _pathFinderNodes[start] = startNode;
            _pathFinderQueue.Enqueue(startNode);
            _visitedCells.Add(start);

            while (_pathFinderQueue.Count > 0)
            {
                var currentNode = _pathFinderQueue.Dequeue();

                if (currentNode.Cell.Equals(end))
                    return OrderClosedNodesAsArray(currentNode);

                ProcessNeighbors(currentNode, end);
            }

            return null;
        }

        public int FindDistance(Vector2Int start, Vector2Int end)
        {
            if (start.Equals(end))
                return 0;

            _pathFinderNodes.Clear();
            _pathFinderQueue.Clear();
            _visitedCells.Clear();

            var h = CalculateHeuristic(start, end);
            var startNode = new PathFinderNode(start, g: 0, h: h, parentNodeCell: start);

            _pathFinderNodes[start] = startNode;
            _pathFinderQueue.Enqueue(startNode);
            _visitedCells.Add(start);

            while (_pathFinderQueue.Count > 0)
            {
                var currentNode = _pathFinderQueue.Dequeue();

                if (currentNode.Cell.Equals(end))
                    return currentNode.G;

                ProcessNeighbors(currentNode, end);
            }

            return -1;
        }

        private void ProcessNeighbors(PathFinderNode currentNode, Vector2Int end)
        {
            var directions = new[]
            {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.right,
                Vector2Int.left,
            };
            
            foreach (var direction in directions)
            {
                var neighborCell = currentNode.Cell + direction;

                if (_gridController.IsOccupied(neighborCell))
                    continue;

                if (_visitedCells.Contains(neighborCell))
                    continue;
                
                var movementCost = 10;
                var newG = currentNode.G + movementCost;

                if (_pathFinderNodes.TryGetValue(neighborCell, out var existingNode))
                {
                    if (newG >= existingNode.G)
                        continue;

                    _pathFinderNodes.Remove(neighborCell);
                }

                var newH = CalculateHeuristic(neighborCell, end);
                var newNode = new PathFinderNode(neighborCell, newG, newH, currentNode.Cell);

                _pathFinderNodes[neighborCell] = newNode;
                _pathFinderQueue.Enqueue(newNode);
                _visitedCells.Add(neighborCell);
            }
        }

        private int CalculateHeuristic(Vector2Int start, Vector2Int end)
        {
            var dx = Mathf.Abs(end.x - start.x);
            var dy = Mathf.Abs(end.y - start.y);

            var newH = (dx + dy) * 10;
            return newH;
        }

        private Vector2Int[] OrderClosedNodesAsArray(PathFinderNode endNode)
        {
            var path = new Stack<Vector2Int>();

            var currentNode = endNode;

            while (currentNode.Cell != currentNode.ParentNodeCell)
            {
                path.Push(currentNode.Cell);
                currentNode = _pathFinderNodes[currentNode.ParentNodeCell];
            }

            path.Push(currentNode.Cell);

            return path.ToArray();
        }
    }
    
    public class ComparePathFinderNodeByFValue : IComparer<PathFinderNode>
    {
        public int Compare(PathFinderNode a, PathFinderNode b)
        {
            if (a.F > b.F)
                return 1;
            if (a.F < b.F)
                return -1;
            return 0;
        }
    }
}