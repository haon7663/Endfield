using Cysharp.Threading.Tasks;

namespace SandyToolkitCore.Settings.Interface
{
    public interface ISettingsRepository
    {
        UniTask<IGameSettings> LoadAsync();
        UniTask<bool> SaveAsync(IGameSettings gameSettings);
    }
}