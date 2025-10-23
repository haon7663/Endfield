using System;
using System.Collections.Generic;
using UnityEngine;
using SandyToolkit.Core;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Threading;
using SandyToolkitCore;
using SandyToolkitCore.Service;
using SandyToolkit.Utility;





#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SandyCore
{
    public abstract class ApplicationManager : MonoBehaviour
    {
        private readonly SandyLogger _logger = new(typeof(ApplicationManager));
        private const string SCENE_CLEANER_NAME = "SceneCleaner";

        private Dictionary<int, BaseController> _modules = new Dictionary<int, BaseController>();
        private Dictionary<Type, BaseController> _modulesByType = new Dictionary<Type, BaseController>();
        private Dictionary<int, int> _updateCounters = new Dictionary<int, int>();
        private Dictionary<int, int> _lateUpdateCounters = new Dictionary<int, int>();
        private Dictionary<int, int> _fixedUpdateCounters = new Dictionary<int, int>();
        private List<BaseController> _sortedModules = new List<BaseController>();
        private bool _isInitialized = false;
        private bool _isSceneLoading = false;

        private HashSet<string> _loadedScenes = new HashSet<string>();
        private HashSet<BaseController> _backProcessControllers = new();
        private Dictionary<Type, ControllerInfo> _controllerInfoCached = new();

        public CancellationTokenSource CancellationTokenSource { get; private set; } = new CancellationTokenSource();

        public static ApplicationManager Instance { get; private set; }


        #region Services
        public ISettingService SettingService { get; private set; }
        public IResourceManageService ResourceManageService { get; private set; }
        public IPoolingService PoolingService { get; private set; }
        public IAudioService AudioService { get; private set; }
        public IEventService EventService { get; private set; }
        public CanvasController CanvasController { get; private set; }
        public UIDocumentController UIDocumentController { get; private set; }
        #endregion


        private Queue<int> _sceneChangeQueue = new Queue<int>();

        #region Unity Events
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _isSceneLoading = false;
                DontDestroyOnLoad(gameObject);
                InitializeAsync().Forget();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            foreach (var module in _sortedModules)
            {
                try
                {
                    if (module.IsDisposed) continue;

                    var controllerInfo = module.ControllerInfo;
                    if (controllerInfo.UpdateInterval <= 0) continue;

                    var index = module.ModuleIndex;
                    if (!_updateCounters.ContainsKey(index))
                    {
                        _updateCounters[index] = 0;
                    }
                    _updateCounters[index]++;

                    if (_updateCounters[index] >= controllerInfo.UpdateInterval)
                    {
                        module.OnUpdate();
                        _updateCounters[index] = 0;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error($"Update Error in {module.ControllerType.Name}: {e.Message}\n{e.StackTrace}");
                }
            }
        }

        private void LateUpdate()
        {
            foreach (var module in _sortedModules)
            {
                try
                {
                    if (module.IsDisposed) continue;

                    var controllerInfo = module.ControllerInfo;
                    if (controllerInfo.LateUpdateInterval <= 0) continue;

                    var index = module.ModuleIndex;
                    if (!_lateUpdateCounters.ContainsKey(index))
                    {
                        _lateUpdateCounters[index] = 0;
                    }
                    _lateUpdateCounters[index]++;

                    if (_lateUpdateCounters[index] >= controllerInfo.LateUpdateInterval)
                    {
                        module.OnLateUpdate();
                        _lateUpdateCounters[index] = 0;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error($"LateUpdate Error in {module.ControllerType.Name}: {e.Message}\n{e.StackTrace}");
                }
            }
        }

        private void FixedUpdate()
        {
            foreach (var module in _sortedModules)
            {
                try
                {
                    if (module.IsDisposed) continue;

                    var controllerInfo = module.ControllerInfo;
                    if (controllerInfo.FixedUpdateInterval <= 0) continue;

                    var index = module.ModuleIndex;
                    if (!_fixedUpdateCounters.ContainsKey(index))
                    {
                        _fixedUpdateCounters[index] = 0;
                    }
                    _fixedUpdateCounters[index]++;

                    if (_fixedUpdateCounters[index] >= controllerInfo.FixedUpdateInterval)
                    {
                        module.OnFixedUpdate();
                        _fixedUpdateCounters[index] = 0;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error($"FixedUpdate Error in {module.ControllerType.Name}: {e.Message}\n{e.StackTrace}");
                }
            }
        }

        private void OnDestroy()
        {
            // 이벤트 해제
            Application.lowMemory -= OnLowMemory;
            Application.deepLinkActivated -= OnDeepLinkActivated;

            foreach (var module in _modules.Values)
            {
                try
                {
                    module.Dispose();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError($"Destroy Error in {module.ControllerType.Name}: {e.Message}\n{e.StackTrace}");
                }
            }

            _modules.Clear();
            _updateCounters.Clear();
            _lateUpdateCounters.Clear();
            _fixedUpdateCounters.Clear();
            _sortedModules.Clear();
        }
        #endregion

        #region MainMethods
        private async UniTaskVoid InitializeAsync()
        {
            // Application 이벤트 등록
            Application.quitting += OnApplicationQuitting;
            Application.lowMemory += OnLowMemory;
            Application.deepLinkActivated += OnDeepLinkActivated;

            _sceneChangeQueue.Clear();

            await HandleBackProcessControllers();

            // 현재 씬에 필요한 컨트롤러 초기화
            var currentScene = SceneManager.GetActiveScene();
            await ResourceManageService.SceneResourceChange(null, currentScene.name);
            PoolingService.ClearAllPool();
            CachedComponents.Reset();

            await CreateControllersForScene(currentScene.name, true);
            CanvasController.LoadSceneCanvases(currentScene.name);
            UIDocumentController.LoadSceneViews(currentScene.name);
            HandleSceneChangeQueue(currentScene.name);

            _isInitialized = true;
        }


        private void SortModules()
        {
            _sortedModules.Clear();
            _sortedModules.AddRange(_modules.Values);
            _sortedModules.Sort((a, b) => a.ControllerInfo.Priority.CompareTo(b.ControllerInfo.Priority));
        }

        public T GetModule<T>() where T : BaseController
        {
            if (_modulesByType.TryGetValue(typeof(T), out var module))
            {
                return module as T;
            }

            return null;
        }

        public async UniTask RegisterModule(BaseController module)
        {
            var index = module.ModuleIndex;
            if (!_modules.ContainsKey(index))
            {
                _logger.Info($"RegisterModule: {module.ControllerType.Name}");
                _modules.Add(index, module);
                _modulesByType[module.ControllerType] = module;
                _sceneChangeQueue.Enqueue(index);
                _updateCounters[index] = 0;
                _lateUpdateCounters[index] = 0;
                _fixedUpdateCounters[index] = 0;
                SortModules();

                if (module.IsInitialized.Equals(false))
                {
                    module.OnInitialize();
                }
            }

            await UniTask.WaitUntil(() => module.IsInitialized, cancellationToken: CancellationTokenSource.Token);
        }

        public void UnregisterModule(BaseController module)
        {
            var index = module.ModuleIndex;
            if (_modules.ContainsKey(index))
            {
                module.Dispose();
                _modules.Remove(index);
                _modulesByType.Remove(module.ControllerType);
                _updateCounters.Remove(index);
                _lateUpdateCounters.Remove(index);
                _fixedUpdateCounters.Remove(index);
                SortModules();
            }
        }

        protected virtual void OnHandleBackProcessController(BaseController controller)
        {
        }

        protected virtual void OnHandleSceneController(BaseController controller)
        {
        }

        private async UniTask HandleBackProcessControllers()
        {
            // 현재 로드된 씬 목록 업데이트
            _loadedScenes.Clear();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                _loadedScenes.Add(SceneManager.GetSceneAt(i).name);
            }

            List<UniTask> tasks = new List<UniTask>();

            // 백그라운드 프로세스 컨트롤러 처리
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                try
                {
                    var controllerTypes = assembly.GetTypes()
                        .Where(t => typeof(BaseController).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract && t.IsSubclassOf(typeof(BaseController)));

                    List<BaseController> instances = new List<BaseController>();

                    foreach (var type in controllerTypes)
                    {
                        BaseController instance = null;

                        try
                        {
                            if (_backProcessControllers.Any(c => c.ControllerType == type))
                            {
                                continue;
                            }

                            if (_controllerInfoCached.TryGetValueIL2CPP(type, out ControllerInfo controllerInfo).Equals(false))
                            {
                                instance = Activator.CreateInstance(type) as BaseController;

                                if (instance == null)
                                {
                                    _logger.Error($"Create Instance Error: {type.Name}");
                                    continue;
                                }

                                _controllerInfoCached[type] = instance.ControllerInfo;
                                controllerInfo = instance.ControllerInfo;
                            }


                            if (controllerInfo.IsBackProcess)
                            {
                                if (instance == null)
                                {
                                    instance = Activator.CreateInstance(type) as BaseController;
                                }

                                if (instance == null)
                                {
                                    _logger.Error($"Create Instance Error: {type.Name}");
                                    continue;
                                }

                                tasks.Add(RegisterModule(instance));

                                if (SettingService == null && instance is ISettingService settingService)
                                {
                                    SettingService = settingService;
                                }

                                if (ResourceManageService == null && instance is IResourceManageService resourceManageService)
                                {
                                    ResourceManageService = resourceManageService;
                                }

                                if (PoolingService == null && instance is IPoolingService poolingService)
                                {
                                    PoolingService = poolingService;
                                }

                                if (AudioService == null && instance is IAudioService audioService)
                                {
                                    AudioService = audioService;
                                }

                                if (EventService == null && instance is IEventService eventService)
                                {
                                    EventService = eventService;
                                }

                                if (CanvasController == null && instance is CanvasController canvasController)
                                {
                                    CanvasController = canvasController;
                                }

                                if (UIDocumentController == null && instance is UIDocumentController uidocumentController)
                                {
                                    UIDocumentController = uidocumentController;
                                }

                                OnHandleBackProcessController(instance);

                                _backProcessControllers.Add(instance);
                                _logger.Info($"Add BackgroundProces Controller: {type.Name}");
                            }
                            else if (instance != null)
                            {
                                // 이미 등록된 컨트롤러라면 Dispose
                                instance.Dispose();
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.Error($"Add BackgroundProces Controller Error: {type.Name}, Error: {e.Message}\n{e.StackTrace}");
                            instance?.Dispose();
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Error($"Search Assembly Error: {assembly.FullName}, Error: {e.Message}, StackTrace: {e.StackTrace}");
                }
            }

            await UniTask.WhenAll(tasks);

            if (SettingService != null && AudioService != null)
            {
                AudioService.Initialize(SettingService.AudioSetting);
            }

            _logger.Info("Initialize BackgroundProces Controller Complete");
        }

        private async UniTask CreateControllersForScene(string sceneName, bool isInitialLoad)
        {
            if (!isInitialLoad)
            {
                // 현재 등록된 모든 컨트롤러 해제 (초기 로드가 아닐 경우에만)
                var controllersToRemove = _modules.Values.Where(c => !c.ControllerInfo.IsBackProcess).ToList();
                foreach (var controller in controllersToRemove)
                {
                    UnregisterModule(controller);
                }
            }

            _logger.Info($"{(isInitialLoad ? "Initialize" : "Create")} Controller for Scene: {sceneName}");

            List<UniTask> tasks = new List<UniTask>();

            // 새로운 씬에 필요한 컨트롤러 생성
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                try
                {
                    var controllerTypes = assembly.GetTypes()
                        .Where(t => typeof(BaseController).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var type in controllerTypes)
                    {
                        BaseController instance = null;
                        try
                        {
                            if (_controllerInfoCached.TryGetValueIL2CPP(type, out ControllerInfo controllerInfo).Equals(false))
                            {
                                instance = Activator.CreateInstance(type) as BaseController;

                                if (instance == null)
                                {
                                    _logger.Error($"Create Instance Error: {type.Name}");
                                    continue;
                                }

                                _controllerInfoCached[type] = instance.ControllerInfo;
                                controllerInfo = instance.ControllerInfo;
                            }

                            if (controllerInfo.IsBackProcess.Equals(false) && controllerInfo.ContainSceneNames.Contains(sceneName))
                            {
                                if (instance == null)
                                {
                                    instance = Activator.CreateInstance(type) as BaseController;
                                }

                                if (instance == null)
                                {
                                    _logger.Error($"Create Instance Error: {type.Name}");
                                    continue;
                                }

                                OnHandleSceneController(instance);

                                tasks.Add(RegisterModule(instance));
                                _logger.Info($"{(isInitialLoad ? "Initialize" : "Create")} Controller for Scene: {type.Name} for scene: {sceneName}");
                            }
                            else if (instance != null)
                            {
                                instance.Dispose();
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.Error($"{(isInitialLoad ? "Initialize" : "Create")} Controller Error: {type.Name}, Error: {e.Message}\n{e.StackTrace}");
                            instance?.Dispose();
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Error($"Search Assembly Error: {assembly.FullName}, Error: {e.Message}, StackTrace: {e.StackTrace}");
                }
            }

            await UniTask.WhenAll(tasks);

            _logger.Info($"{(isInitialLoad ? "Initialize" : "Create")} Controller for Scene Complete: {sceneName}");
        }
        #endregion

        #region SceneControl
        public async UniTask LoadScene(string sceneName)
        {
            await UniTask.WaitUntil(() => _isInitialized);
            await UniTask.WaitUntil(() => _isSceneLoading.Equals(false));

            _sceneChangeQueue.Clear();

            _isSceneLoading = true;

            // 현재 씬 이름 저장
            var currentScene = SceneManager.GetActiveScene().name;

            // 1. SceneCleaner 씬 로드
            await SceneManager.LoadSceneAsync(SCENE_CLEANER_NAME, LoadSceneMode.Single);

            // 2. 백그라운드 프로세스 컨트롤러 처리
            await HandleBackProcessControllers();

            // 3. Addressable 리소스 변경
            await ResourceManageService.SceneResourceChange(currentScene, sceneName);
            PoolingService.ClearAllPool();
            EventService.Clear();
            CachedComponents.Reset();

            // 4. 새로운 씬에 필요한 컨트롤러 생성
            await CreateControllersForScene(sceneName, false);
            // 5. 새로운 씬 로드
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            CanvasController.LoadSceneCanvases(sceneName);
            UIDocumentController.LoadSceneViews(sceneName);

            HandleSceneChangeQueue(sceneName);

            _isSceneLoading = false;
        }

        private void HandleSceneChangeQueue(string sceneName)
        {
            while (_sceneChangeQueue.Count > 0)
            {
                var index = _sceneChangeQueue.Dequeue();
                _modules[index].OnSceneLoaded(sceneName);
            }
        }
        #endregion

        #region Application Events
        private void OnApplicationQuitting()
        {
            _logger.Info("OnApplicationQuitting");

            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();

            foreach (var module in _sortedModules)
            {
                try
                {
                    if (module.IsDisposed) continue;
                    module.Dispose();
                }
                catch (Exception e)
                {
                    _logger.Error($"Application Quitting Error in {module.ControllerType.Name}: {e.Message}\n{e.StackTrace}");
                }
            }

            Instance = null;
            SandyLogger.CloseSharedWriter().Forget();

            Application.quitting -= OnApplicationQuitting;
        }

        private void OnLowMemory()
        {
            _logger.Warning("OnLowMemory");

            foreach (var module in _sortedModules)
            {
                try
                {
                    if (module.IsDisposed) continue;
                    // 메모리 정리 로직
                }
                catch (Exception e)
                {
                    _logger.Error($"Low Memory Error in {module.ControllerType.Name}: {e.Message}\n{e.StackTrace}");
                }
            }
        }

        private void OnDeepLinkActivated(string url)
        {
            _logger.Info($"OnDeepLinkActivated: {url}");

            foreach (var module in _sortedModules)
            {
                try
                {
                    if (module.IsDisposed) continue;
                    // 딥링크 처리 로직
                }
                catch (Exception e)
                {
                    _logger.Error($"Deep Link Error in {module.ControllerType.Name}: {e.Message}\n{e.StackTrace}");
                }
            }
        }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            AddSceneCleanerToBuildSettings();
        }

        private void Reset()
        {
            AddSceneCleanerToBuildSettings();
        }

        private void AddSceneCleanerToBuildSettings()
        {
            // 가능한 씬 경로들
            string[] possibleScenePaths = new string[]
            {
                $"Packages/com.sandyfloor.sandytoolkit/Scenes/{SCENE_CLEANER_NAME}.unity",
                $"Assets/Plugins/SandyToolkit/Scenes/{SCENE_CLEANER_NAME}.unity"
            };

            string scenePath = null;
            foreach (var path in possibleScenePaths)
            {
                try
                {
                    if (SceneHelper.IsSceneExists(path))
                    {
                        scenePath = path;
                        break;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error($"Search Scene Error: {path}, Error: {e.Message}\n{e.StackTrace}");
                    continue;
                }
            }

            if (string.IsNullOrEmpty(scenePath))
            {
                _logger.Error($"{SCENE_CLEANER_NAME} Scene Not Found. Please Check the following paths:");
                foreach (var path in possibleScenePaths)
                {
                    _logger.Error($"- {path}");
                }
                return;
            }

            SceneHelper.AddSceneToBuildSettings(scenePath);
        }
#endif
    }
}