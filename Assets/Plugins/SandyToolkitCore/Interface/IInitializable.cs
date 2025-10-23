using Cysharp.Threading.Tasks;

namespace SandyToolkit.Core
{
    public interface IInitializable
    {
        public UniTask Initialize();
    }
}