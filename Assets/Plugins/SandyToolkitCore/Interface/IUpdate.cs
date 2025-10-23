using System;

namespace SandyToolkit.Core
{
    public interface IClassUpdate
    {
        public void ExecuteUpdate();
    }
    public interface IClassFixedUpdate
    {
        public void ExecuteFixedUpdate();
    }
    public interface IClassLateUpdate
    {
        public void ExecuteLateUpdate();
    }
}