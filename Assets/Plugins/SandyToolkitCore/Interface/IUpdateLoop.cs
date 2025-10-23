using UnityEngine;

namespace SandyToolkit.Core
{
    public interface IUpdateLoop
    {
        GameObject gameObject { get; }
        void ExecuteUpdate(float timeDelta);
    }

    public interface IFixedUpdateLoop
    {
        GameObject gameObject { get; }
        void ExecuteFixedUpdate(float timeDelta);
    }

    public interface ILateUpdateLoop
    {
        GameObject gameObject { get; }
        void ExecuteLateUpdate(float timeDelta);
    }
}