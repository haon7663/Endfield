using System;
using System.Collections.Generic;
using SandyToolkit.Utility;
using SandyToolkitCore;
using UnityEngine;

namespace Core.Controllers
{
    public class UnitController : BaseController
    {
        public override ControllerInfo ControllerInfo => new ControllerInfo
        {
            ContainSceneNames = new string[] { "Gameplay" },
            Priority = 1,
            UpdateInterval = 0,
            LateUpdateInterval = 0,
            FixedUpdateInterval = 0,
            IsBackProcess = false
        };
        
        private readonly Dictionary<int, object> _references = new();
        private int[] _idArray = new int[16];
        private uint _count;

        public void Register(int instanceId, object obj)
        {
            if (_count >= _idArray.Length)
            {
                var newSize = _idArray.Length * 2;
                var newArray = new int[newSize];

                Array.Copy(_idArray, newArray, _idArray.Length);

                _idArray = newArray;
            }

            _idArray[_count++] = instanceId;
            _references[instanceId] = obj;
        }

        public void UnRegister(int instanceId)
        {
            for (var i = 0; i < _count; i++)
            {
                if (_idArray[i] == instanceId)
                {
                    if (i < _count - 1)
                    {
                        _idArray[i] = _idArray[_count - 1];
                    }
                    _idArray[_count - 1] = -1;
                    _count--;
                    break;
                }
            }

            _references.Remove(instanceId);
        }
        
        public IEnumerable<object> GetEnumerable()
        {
            return _references.Values;
        }

        public object Get(int id)
        {
            return _references.GetValueOrDefault(id);
        }

        public bool TryGet(int id, out object result)
        {
            return _references.TryGetValue(id, out result);
        }

        public T Get<T>(int id) where T : class
        {
            if (!_references.TryGetValue(id, out var obj))
            {
                return null;
            }

#if DEVELOPMENT_BUILD || UNITY_EDITOR
            if (obj is not T)
            {
                Debug.LogWarning($"Object with ID {id} is not of type {typeof(T).Name}");
                return null;
            }
#endif

            return UnsafeHelper.UnsafeAs<object, T>(obj);
        }

        public bool TryGet<T>(int id, out T result) where T : class
        {
            if (!_references.TryGetValue(id, out var obj))
            {
                result = null;
                return false;
            }

#if DEVELOPMENT_BUILD || UNITY_EDITOR
            if (obj is not T)
            {
                Debug.LogWarning($"Object with ID {id} is not of type {typeof(T).Name}");
                result = null;
                return false;
            }
#endif
            result = UnsafeHelper.UnsafeAs<object, T>(obj);
            return true;
        }
    }
}