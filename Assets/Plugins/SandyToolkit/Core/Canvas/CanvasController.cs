using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SandyToolkitCore;
using SandyToolkitCore.Service;
using SandyToolkit.Core.Canvas;
using static ViewCanvas;
using System.Collections;

public class CanvasController : BaseController
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

    private readonly Dictionary<Type, ViewCanvas> _viewCanvasMap = new();
    private readonly Dictionary<string, ViewCanvas> _viewCanvasMapString = new();
    private readonly List<ViewCanvas> _visibleViewCanvasList = new();
    private readonly List<ViewCanvas> _visibleOverlayViewCanvasList = new();
    private readonly List<ViewCanvas> _visibleCamOverlayViewCanvasList = new();

    private int _closableCount;
    private CanvasLoader _canvasLoader;

    public bool IsClosableExist => _closableCount > 0;

    public override void OnInitialize()
    {
        _closableCount = 0;
        _viewCanvasMap.Clear();
        _viewCanvasMapString.Clear();
        _visibleViewCanvasList.Clear();
        _visibleOverlayViewCanvasList.Clear();
        _visibleCamOverlayViewCanvasList.Clear();

        if (SortingLayer.NameToID(ViewCanvas.UILayerName) == 0)
        {
            Debug.LogError($"Unity에서 {ViewCanvas.UILayerName} SortingLayer가 없습니다. Unity Editor에서 Edit > Project Settings > Tags and Layers에서 {ViewCanvas.UILayerName} SortingLayer를 추가해주세요.");
        }

        if (SortingLayer.NameToID(ViewCanvas.UIOverlayLayerName) == 0)
        {
            Debug.LogError($"Unity에서 {ViewCanvas.UIOverlayLayerName} SortingLayer가 없습니다. Unity Editor에서 Edit > Project Settings > Tags and Layers에서 {ViewCanvas.UIOverlayLayerName} SortingLayer를 추가해주세요.");
        }

        // CanvasLoader 로드
        _canvasLoader = Resources.Load<CanvasLoader>("Loader/CanvasLoader");
        if (_canvasLoader == null)
        {
            Debug.LogError("CanvasLoader를 찾을 수 없습니다. Resources/Loader 폴더에 CanvasLoader가 있는지 확인하세요.");
        }

        IsInitialized = true;
    }

    public ViewCanvas GetTopCanvas()
    {
        if (_visibleOverlayViewCanvasList != null && _visibleOverlayViewCanvasList.Count > 0)
        {
            return _visibleOverlayViewCanvasList.Last();
        }

        if (_visibleViewCanvasList != null && _visibleViewCanvasList.Count > 0)
        {
            return _visibleViewCanvasList.Last();
        }

        return null;
    }

    public ViewCanvas GetTopClosableCanvas()
    {
        if (IsClosableExist.Equals(false)) return null;
        var topArray = GetTopClosableCanvasArray();
        return topArray?.Last();
    }

    public ViewCanvas[] GetTopClosableCanvasArray()
    {
        if (IsClosableExist.Equals(false)) return null;

        var closableCanvases = GetOnlyCloasableCanvases(_visibleOverlayViewCanvasList);
        if (closableCanvases != null)
        {
            return closableCanvases;
        }

        closableCanvases = GetOnlyCloasableCanvases(_visibleViewCanvasList);
        return closableCanvases;
    }

    private ViewCanvas[] GetOnlyCloasableCanvases(List<ViewCanvas> canvasList)
    {
        var closableCanvases = canvasList.Where((target) => target.IsCloseable);
        var orderedCloasableCanvases = closableCanvases.OrderBy((target) => target.ThisCanvas.sortingOrder);
        var viewCanvasArray = orderedCloasableCanvases.ToArray();
        return viewCanvasArray.Length > 0 ? viewCanvasArray : null;
    }

    public bool ExitCurrentCanvas()
    {
        var viewCanvases = GetTopClosableCanvasArray();
        if (viewCanvases == null) return false;

        var topCanvas = viewCanvases.Last();
        topCanvas.SetVisible(false);
        return true;
    }

    public T Get<T>() where T : ViewCanvas
    {
        if (_viewCanvasMap.ContainsKey(typeof(T)).Equals(false))
        {
            return null;
        }
        return _viewCanvasMap[typeof(T)] as T;
    }

    public ViewCanvas Get(Type type)
    {
        if (_viewCanvasMap.ContainsKey(type).Equals(false))
        {
            return null;
        }
        return _viewCanvasMap[type];
    }

    public ViewCanvas Get(string name)
    {
        if (_viewCanvasMapString.ContainsKey(name).Equals(false))
        {
            return null;
        }
        return _viewCanvasMapString[name];
    }

    public void PopAll()
    {
        foreach (var item in _visibleViewCanvasList.Where((target) => target.IsCloseable).ToArray())
        {
            item.SetVisible(false, false);
        }
        _closableCount = 0;
    }

    protected void RegisterCanvas(ViewCanvas canvas)
    {
        var type = canvas.GetType();
        var targetName = type.Name;

        if (!_viewCanvasMapString.ContainsKey(targetName))
        {
            _viewCanvasMapString.Add(targetName, canvas);
            Debug.Log($"<color=#f0f>Setup Key({targetName}) :: MapString</color>");
        }

        if (!_viewCanvasMap.ContainsKey(type))
        {
            _viewCanvasMap.Add(type, canvas);
            Debug.Log($"<color=#f0f>Setup Key</color>");
        }

        canvas.Setup();
        canvas.SetVisible(false, false);
    }

    public void UnregisterCanvas(ViewCanvas canvas)
    {
        var type = canvas.GetType();
        var targetName = type.Name;

        if (_viewCanvasMapString.ContainsKey(targetName))
        {
            _viewCanvasMapString.Remove(targetName);
        }

        if (_viewCanvasMap.ContainsKey(type))
        {
            _viewCanvasMap.Remove(type);
        }
    }

    public void AddToVisibleList(ViewCanvas canvas)
    {
        var canvasType = canvas.CanvasType;
        List<ViewCanvas> targetCanvasList = canvasType switch
        {
            ECanvasType.Overlay => _visibleOverlayViewCanvasList,
            ECanvasType.CameraOverlay => _visibleCamOverlayViewCanvasList,
            _ => _visibleViewCanvasList,
        };

        if (targetCanvasList.Contains(canvas)) return;
        targetCanvasList.Add(canvas);

        SortCanvasList(targetCanvasList);

        if (canvas.IsCloseable)
        {
            _closableCount++;
        }
    }

    private void SortCanvasList(List<ViewCanvas> canvasList)
    {
        for (int i = 0; i < canvasList.Count; i++)
        {
            canvasList[i].ThisCanvas.sortingOrder = i;
        }
    }

    public void RemoveFromVisibleList(ViewCanvas canvas)
    {
        var canvasType = canvas.CanvasType;
        List<ViewCanvas> targetCanvasList = canvasType switch
        {
            ECanvasType.Overlay => _visibleOverlayViewCanvasList,
            ECanvasType.CameraOverlay => _visibleCamOverlayViewCanvasList,
            _ => _visibleViewCanvasList,
        };

        if (targetCanvasList.Remove(canvas).Equals(false)) return;

        SortCanvasList(targetCanvasList);

        if (canvas.IsCloseable)
        {
            _closableCount--;
        }
    }

    public void LoadSceneCanvases(string sceneName)
    {
        if (_canvasLoader == null) return;

        var sceneCanvases = _canvasLoader.GetCanvasesForScene(sceneName);
        foreach (var canvasInfo in sceneCanvases)
        {
            var canvas = UnityEngine.Object.Instantiate(canvasInfo.Prefab);
            var viewCanvas = canvas.GetComponent<ViewCanvas>();
            if (viewCanvas != null)
            {
                RegisterCanvas(viewCanvas);
            }
        }
    }

    protected override void OnDispose()
    {
        PopAll();
        _viewCanvasMap.Clear();
        _viewCanvasMapString.Clear();
        _visibleViewCanvasList.Clear();
        _visibleOverlayViewCanvasList.Clear();
        _closableCount = 0;
    }
}