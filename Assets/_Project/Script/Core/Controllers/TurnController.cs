using System;
using SandyCore;
using SandyToolkitCore;
using UnityEngine;

namespace Core.Controllers
{
    public class TurnController : BaseController
    {
        public override ControllerInfo ControllerInfo => new ControllerInfo
        {
            ContainSceneNames = new string[] { "Gameplay" },
            Priority = 0,
            UpdateInterval = 1,
            LateUpdateInterval = 0,
            FixedUpdateInterval = 0,
            IsBackProcess = false
        };
        
        public int TurnCount { get; private set; }
        public event Action<int> OnTurnChanged;

        private float _turnProcess;

        public override void OnSceneLoaded(string sceneName)
        {
            base.OnSceneLoaded(sceneName);

            ApplicationManager.Instance.PoolingService.GetPoolable<Domain.Units.Unit>("Player", new Vector3(0, 0.5f));
            ApplicationManager.Instance.PoolingService.GetPoolable<Domain.Units.Unit>("Enemy", new Vector3(5, 0.5f));
            ApplicationManager.Instance.PoolingService.GetPoolable<Domain.Units.Unit>("Enemy", new Vector3(2, 0.5f));
            ApplicationManager.Instance.PoolingService.GetPoolable<Domain.Units.Unit>("Enemy", new Vector3(-4, 0.5f));
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            _turnProcess += Time.deltaTime;
            if (_turnProcess >= 1f)
            {
                _turnProcess -= 1f;
                ChangeTurn();
            }
        }
        
        private void ChangeTurn()
        {
            TurnCount += 1;
            OnTurnChanged?.Invoke(TurnCount);
            
            Debug.Log($"Turn changed to {TurnCount}");
        }

        public void ForceChangeTurn()
        {
            _turnProcess = 0;
            ChangeTurn();
            
            Debug.Log($"Force changed turn");
        }
    }
}