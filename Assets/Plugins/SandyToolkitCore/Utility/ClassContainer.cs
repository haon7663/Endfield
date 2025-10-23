using System;
using System.Collections.Generic;

namespace SandyToolkit.Utility
{
    public class ClassContainer<TK, T> : IDisposable where T : class
    {
        private const int INIT_COUNT = 20;
        private const float GROWTH_FACTOR = 1.5f;

        private int _pointer;
        private T[] _elements;
        private readonly Dictionary<TK, int> _keyByIndexDic = new();
        private readonly Queue<int> _emptyIndices = new();
        private T[] _compactBuffer; // ReadOnlySpan용 임시 버퍼

        public int Count => _keyByIndexDic.Count;
        public int Capacity => _elements.Length;

        public ClassContainer()
        {
            _pointer = 0;
            _elements = new T[INIT_COUNT];
            _compactBuffer = new T[INIT_COUNT];
            // 초기화 시에는 emptyIndices를 비워둠 - _pointer를 먼저 사용
        }

        public bool ContainsKey(TK key)
        {
            return _keyByIndexDic.ContainsKey(key);
        }

        public void AddElement(TK key, T element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (_keyByIndexDic.ContainsKey(key))
                throw new ArgumentException($"Key {key} already exists in container");

            int index;
            if (_emptyIndices.Count > 0)
            {
                index = _emptyIndices.Dequeue();
            }
            else
            {
                if (_pointer >= _elements.Length)
                {
                    ResizeArray();
                }
                index = _pointer++;
            }

            _elements[index] = element;
            _keyByIndexDic[key] = index;
        }

        private void ResizeArray()
        {
            int oldSize = _elements.Length;
            int newSize = (int)(oldSize * GROWTH_FACTOR);
            var newArray = new T[newSize];
            Array.Copy(_elements, newArray, oldSize);
            _elements = newArray;
        }

        public T GetElement(TK key)
        {
            if (_keyByIndexDic.TryGetValue(key, out int index))
            {
                return _elements[index];
            }
            return null;
        }

        public T GetElementByIndex(int index)
        {
            if (index < 0 || index >= _elements.Length)
                return null;
            return _elements[index];
        }

        /// <summary>
        /// 유효한 요소들의 ReadOnlySpan을 반환합니다. (메모리 할당 없음, 안전한 읽기 전용)
        /// 주의: 반환된 ReadOnlySpan은 일시적이므로 즉시 사용해야 합니다.
        /// </summary>
        public ReadOnlySpan<T> GetValidElementsSpan()
        {
            int count = _keyByIndexDic.Count;
            
            // 버퍼 크기가 부족하면 재할당
            if (count > _compactBuffer.Length)
            {
                _compactBuffer = new T[count];
            }
            
            // 유효한 요소들을 버퍼에 복사
            int index = 0;
            foreach (var elementIndex in _keyByIndexDic.Values)
            {
                _compactBuffer[index++] = _elements[elementIndex];
            }

            // 실제 데이터 부분만 ReadOnlySpan으로 반환
            return new ReadOnlySpan<T>(_compactBuffer, 0, count);
        }

        /// <summary>
        /// 전체 내부 배열의 ReadOnlySpan을 반환합니다. (null 요소 포함)
        /// 성능이 중요하고 null 체크를 직접 할 경우 사용
        /// </summary>
        public ReadOnlySpan<T> GetInternalArraySpan()
        {
            return new ReadOnlySpan<T>(_elements, 0, _pointer);
        }

        /// <summary>
        /// 유효한 요소들의 스냅샷을 새 배열로 반환합니다. (안전한 복사본)
        /// </summary>
        public T[] GetValidElementsSnapshot()
        {
            int count = _keyByIndexDic.Count;
            var snapshot = new T[count];
            
            int index = 0;
            foreach (var elementIndex in _keyByIndexDic.Values)
            {
                snapshot[index++] = _elements[elementIndex];
            }

            return snapshot;
        }

        /// <summary>
        /// 성능이 중요한 경우 사용 - 내부 버퍼를 반환하되 실제 count를 함께 반환
        /// 반환된 배열을 수정하지 마세요!
        /// </summary>
        public (T[] buffer, int count) GetValidElementsBuffer()
        {
            int count = _keyByIndexDic.Count;
            
            // 임시 배열 생성 (매번 새로 만들어서 안전)
            var buffer = new T[count];
            int index = 0;
            foreach (var elementIndex in _keyByIndexDic.Values)
            {
                buffer[index++] = _elements[elementIndex];
            }

            return (buffer, count);
        }

        /// <summary>
        /// ReadOnlySpan을 사용하여 특정 조건을 만족하는 요소를 찾습니다.
        /// </summary>
        public T FindElement(Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var span = GetValidElementsSpan();
            foreach (var element in span)
            {
                if (predicate(element))
                {
                    return element;
                }
            }
            return null;
        }

        /// <summary>
        /// ReadOnlySpan을 사용하여 조건을 만족하는 모든 요소를 찾습니다.
        /// </summary>
        public List<T> FindElements(Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var result = new List<T>();
            var span = GetValidElementsSpan();
            
            foreach (var element in span)
            {
                if (predicate(element))
                {
                    result.Add(element);
                }
            }
            return result;
        }

        /// <summary>
        /// ReadOnlySpan을 사용하여 각 요소에 대해 액션을 실행합니다.
        /// </summary>
        public void ForEach(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var span = GetValidElementsSpan();
            foreach (var element in span)
            {
                action(element);
            }
        }

        /// <summary>
        /// ReadOnlySpan을 사용하여 모든 요소가 조건을 만족하는지 확인합니다.
        /// </summary>
        public bool All(Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var span = GetValidElementsSpan();
            foreach (var element in span)
            {
                if (!predicate(element))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// ReadOnlySpan을 사용하여 조건을 만족하는 요소가 하나라도 있는지 확인합니다.
        /// </summary>
        public bool Any(Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var span = GetValidElementsSpan();
            foreach (var element in span)
            {
                if (predicate(element))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 모든 키를 반환합니다.
        /// </summary>
        public TK[] GetAllKeys()
        {
            var keys = new TK[_keyByIndexDic.Count];
            int index = 0;
            foreach (var key in _keyByIndexDic.Keys)
            {
                keys[index++] = key;
            }
            return keys;
        }

        public void RemoveElement(TK key)
        {
            if (_keyByIndexDic.TryGetValue(key, out int index))
            {
                _elements[index] = null;
                _keyByIndexDic.Remove(key);
                _emptyIndices.Enqueue(index);
            }
        }

        public void ClearContainer()
        {
            Array.Clear(_elements, 0, _elements.Length);
            Array.Clear(_compactBuffer, 0, _compactBuffer.Length);
            _keyByIndexDic.Clear();
            _emptyIndices.Clear();
            _pointer = 0;
        }

        public void Dispose()
        {
            _elements = null;
            _compactBuffer = null;
            _keyByIndexDic?.Clear();
            _emptyIndices?.Clear();
            _pointer = 0;
        }
    }
}