using SandyToolkitCore;
using UnityEngine;

namespace Core.Controllers
{
    public class GameplayController : BaseController
    {
        public override ControllerInfo ControllerInfo => new ControllerInfo
        {
            ContainSceneNames = new string[] { "Gameplay" },
            Priority = 0,
            UpdateInterval = 0,
            LateUpdateInterval = 0,
            FixedUpdateInterval = 0,
            IsBackProcess = false
        };

        public GridController GridController { get; private set; }
        
        public override void OnSceneLoaded(string sceneName)
        {
            base.OnSceneLoaded(sceneName);
            
            ConstructControllers();
            InitializeControllers();
        }

        private void ConstructControllers()
        {
            GridController = new GridController();
        }

        private void InitializeControllers()
        {
            GridController.Initialize(new Vector2Int(9, 4));
        }
    }
}
