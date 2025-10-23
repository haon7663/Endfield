using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SandyToolkitCore;
using SandyToolkitCore.Service;
using SandyToolkit.Core.UI;

public class UIDocumentController : BaseController
{
    public override ControllerInfo ControllerInfo => new()
    {
        ContainSceneNames = new string[] { },
        Priority = 0,
        UpdateInterval = 0,
        LateUpdateInterval = 0,
        FixedUpdateInterval = 0,
        IsBackProcess = true
    };

    private readonly Dictionary<Type, UIDocumentView> _viewMap = new();
    private readonly Dictionary<string, UIDocumentView> _viewMapString = new();
    private readonly List<UIDocumentView> _visibleViewList = new();
    private readonly List<UIDocumentView> _visibleOverlayViewList = new();
    private int _closableCount;
    private UIDocumentLoader _documentLoader;

    public bool IsClosableExist => _closableCount > 0;

    public override void OnInitialize()
    {
        _closableCount = 0;
        _viewMap.Clear();
        _viewMapString.Clear();
        _visibleViewList.Clear();
        _visibleOverlayViewList.Clear();

        // UIDocumentLoader 로드
        _documentLoader = Resources.Load<UIDocumentLoader>("Loader/UIDocumentLoader");
        if (_documentLoader == null)
        {
            Debug.LogError("UIDocumentLoader를 찾을 수 없습니다. Resources 폴더에 UIDocumentLoader가 있는지 확인하세요.");
        }

        IsInitialized = true;
    }

    public UIDocumentView GetTopView()
    {
        if (_visibleOverlayViewList != null && _visibleOverlayViewList.Count > 0)
        {
            return _visibleOverlayViewList.Last();
        }

        if (_visibleViewList != null && _visibleViewList.Count > 0)
        {
            return _visibleViewList.Last();
        }

        return null;
    }

    public UIDocumentView GetTopClosableView()
    {
        if (IsClosableExist.Equals(false)) return null;
        var topArray = GetTopClosableViewArray();
        return topArray?.Last();
    }

    public UIDocumentView[] GetTopClosableViewArray()
    {
        if (IsClosableExist.Equals(false)) return null;

        var closableViews = GetOnlyClosableViews(_visibleOverlayViewList);
        if (closableViews != null)
        {
            return closableViews;
        }

        closableViews = GetOnlyClosableViews(_visibleViewList);
        return closableViews;
    }

    private UIDocumentView[] GetOnlyClosableViews(List<UIDocumentView> viewList)
    {
        var closableViews = viewList.Where((target) => target.IsCloseable);
        var viewArray = closableViews.ToArray();
        return viewArray.Length > 0 ? viewArray : null;
    }

    public bool ExitCurrentView()
    {
        var views = GetTopClosableViewArray();
        if (views == null) return false;

        var topView = views.Last();
        topView.SetVisible(false);
        return true;
    }

    public T Get<T>() where T : UIDocumentView
    {
        if (_viewMap.ContainsKey(typeof(T)).Equals(false))
        {
            return null;
        }
        return _viewMap[typeof(T)] as T;
    }

    public UIDocumentView Get(Type type)
    {
        if (_viewMap.ContainsKey(type).Equals(false))
        {
            return null;
        }
        return _viewMap[type];
    }

    public UIDocumentView Get(string name)
    {
        if (_viewMapString.ContainsKey(name).Equals(false))
        {
            return null;
        }
        return _viewMapString[name];
    }

    public void PopAll()
    {
        foreach (var item in _visibleViewList.Where((target) => target.IsCloseable).ToArray())
        {
            item.SetVisible(false);
        }
        _closableCount = 0;
    }

    protected void RegisterView(UIDocumentView view)
    {
        var type = view.GetType();
        var targetName = type.Name;

        if (!_viewMapString.ContainsKey(targetName))
        {
            _viewMapString.Add(targetName, view);
            Debug.Log($"<color=#f0f>Setup Key({targetName}) :: MapString</color>");
        }

        if (!_viewMap.ContainsKey(type))
        {
            _viewMap.Add(type, view);
            Debug.Log($"<color=#f0f>Setup Key</color>");
        }

        view.Setup();
        view.SetVisible(false);
    }

    public void UnregisterView(UIDocumentView view)
    {
        var type = view.GetType();
        var targetName = type.Name;

        if (_viewMapString.ContainsKey(targetName))
        {
            _viewMapString.Remove(targetName);
        }

        if (_viewMap.ContainsKey(type))
        {
            _viewMap.Remove(type);
        }
    }

    public void AddToVisibleList(UIDocumentView view, bool isOverlay)
    {
        if (isOverlay)
        {
            _visibleOverlayViewList.Add(view);
        }
        else
        {
            _visibleViewList.Add(view);
        }

        if (view.IsCloseable)
        {
            _closableCount++;
        }
    }

    public void RemoveFromVisibleList(UIDocumentView view)
    {
        bool removed = _visibleOverlayViewList.Remove(view) ||
                      _visibleViewList.Remove(view);

        if (view.IsCloseable && removed)
        {
            _closableCount--;
        }
    }

    public void LoadSceneViews(string sceneName)
    {
        if (_documentLoader == null) return;

        var sceneViews = _documentLoader.GetUIDocumentsForScene(sceneName);
        foreach (var viewInfo in sceneViews)
        {
            var view = UnityEngine.Object.Instantiate(viewInfo.Prefab);
            var uiDocumentView = view.GetComponent<UIDocumentView>();
            if (uiDocumentView != null)
            {
                RegisterView(uiDocumentView);
            }
        }
    }

    protected override void OnDispose()
    {
        PopAll();
        _viewMap.Clear();
        _viewMapString.Clear();
        _visibleViewList.Clear();
        _visibleOverlayViewList.Clear();
        _closableCount = 0;
    }
} 