using System.Collections.Generic;

namespace Utils
{
    public class PriorityQueue<T>
    {
        private readonly List<T> _heap;
        private readonly IComparer<T> _comparer;
        private readonly int _maxSize;

        public int Count => _heap.Count;
        public bool IsFull => _heap.Count >= _maxSize;
        public int MaxSize => _maxSize;

        public PriorityQueue(int maxSize, IComparer<T> comparer)
        {
            _maxSize = maxSize > 0 ? maxSize : int.MaxValue;
            _heap = new List<T>(maxSize > 0 ? maxSize : 16);
            _comparer = comparer;
        }

        public void Clear() => _heap.Clear();

        public bool TryEnqueue(T item, out T replacedItem)
        {
            replacedItem = default;
            
            if (_heap.Count >= _maxSize)
            {
                if (_comparer.Compare(item, _heap[0]) > 0)
                {
                    replacedItem = _heap[0];
                    _heap[0] = item;
                    HeapifyDown(0);
                    return true;
                }
                return false;
            }
            
            _heap.Add(item);
            HeapifyUp(_heap.Count - 1);
            return true;
        }

        public void Enqueue(T item)
        {
            if (_heap.Count >= _maxSize)
            {
                if (_comparer.Compare(item, _heap[0]) > 0)
                {
                    _heap[0] = item;
                    HeapifyDown(0);
                }
                return;
            }
            _heap.Add(item);
            HeapifyUp(_heap.Count - 1);
        }

        public T Peek()
        {
            if (_heap.Count == 0)
                throw new System.InvalidOperationException("Queue is empty");
                
            return _heap[0];
        }

        public bool TryPeek(out T result)
        {
            if (_heap.Count == 0)
            {
                result = default;
                return false;
            }
            
            result = _heap[0];
            return true;
        }

        public T Dequeue()
        {
            if (_heap.Count == 0) 
                throw new System.InvalidOperationException("Queue is empty");

            T result = _heap[0];
            _heap[0] = _heap[^1];
            _heap.RemoveAt(_heap.Count - 1);

            if (_heap.Count > 0)
                HeapifyDown(0);

            return result;
        }

        public bool TryDequeue(out T result)
        {
            if (_heap.Count == 0)
            {
                result = default;
                return false;
            }

            result = _heap[0];
            _heap[0] = _heap[^1];
            _heap.RemoveAt(_heap.Count - 1);

            if (_heap.Count > 0)
                HeapifyDown(0);

            return true;
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (_comparer.Compare(_heap[index], _heap[parentIndex]) >= 0)
                    break;

                Swap(index, parentIndex);
                index = parentIndex;
            }
        }

        private void HeapifyDown(int index)
        {
            while (true)
            {
                int smallest = index;
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;

                if (leftChild < _heap.Count && _comparer.Compare(_heap[leftChild], _heap[smallest]) < 0)
                    smallest = leftChild;

                if (rightChild < _heap.Count && _comparer.Compare(_heap[rightChild], _heap[smallest]) < 0)
                    smallest = rightChild;

                if (smallest == index)
                    break;

                Swap(index, smallest);
                index = smallest;
            }
        }

        private void Swap(int i, int j)
        {
            (_heap[i], _heap[j]) = (_heap[j], _heap[i]);
        }
    }
}