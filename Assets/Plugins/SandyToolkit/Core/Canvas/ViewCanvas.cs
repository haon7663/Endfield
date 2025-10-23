using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SandyToolkit.Utility;
using UnityEngine;
using UnityEngine.Events;
using SandyToolkitCore;
using SandyCore;

public abstract class ViewCanvas : MonoBehaviour
{
    public enum ECanvasType
    {
        Normal,
        Overlay,
        CameraOverlay,
    }

    public const string MainCameraTag = "MainCamera";
    public const string UIOverlayLayerName = "UIOverlay";
    public const string UILayerName = "UI";

    protected virtual string TargetCameraTag => MainCameraTag;

    [field: SerializeField] public bool IsCloseable { get; protected set; }
    [field: SerializeField] public ECanvasType CanvasType { get; protected set; }
    [field: SerializeField] public bool UseSafeArea { get; protected set; }

    private RectTransform _safeAreaRect;

    private const int OverlayLayer = 100;
    public UnityEngine.Canvas ThisCanvas { get; protected set; }
    public RectTransform ThisRectTransform { get; protected set; }
    public CanvasGroup ThisCanvasGroup { get; private set; }

    public bool IsVisible { get; protected set; }

    private UnityAction<bool> _onChangeVisible;
    private Coroutine _visibleCoroutine;
    private Rect _lastSafeArea = Rect.zero;
    private Vector2Int _lastScreenSize = Vector2Int.zero;

    private void OnDestroy()
    {
        if (CanvasType != ECanvasType.CameraOverlay)
        {
            if (ApplicationManager.Instance != null)
            {
                var canvasController = ApplicationManager.Instance.GetModule<CanvasController>();
                if (canvasController != null)
                {
                    canvasController.UnregisterCanvas(this);
                    canvasController.RemoveFromVisibleList(this);
                }
            }
            Dispose();
        }
    }

    public void Setup()
    {
        if (ThisCanvas == null)
            ThisCanvas = GetComponent<Canvas>();

        if (ThisRectTransform == null)
            ThisRectTransform = GetComponent<RectTransform>();

        if (ThisCanvas.worldCamera == null)
            ThisCanvas.worldCamera =
                GameObject.FindGameObjectWithTag(TargetCameraTag).GetComponent<Camera>() as Camera;

        if (ThisCanvasGroup == null)
            ThisCanvasGroup = GetComponent<CanvasGroup>();

        if (ThisCanvasGroup == null)
        {
            ThisCanvasGroup = gameObject.AddComponent<CanvasGroup>();
            ThisCanvasGroup.alpha = 0f;
            ThisCanvasGroup.interactable = false;
            ThisCanvasGroup.blocksRaycasts = false;
            ThisCanvasGroup.ignoreParentGroups = true;
        }

        if (CanvasType == ECanvasType.CameraOverlay)
        {
            ThisCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        else
        {
            ThisCanvas.renderMode = RenderMode.ScreenSpaceCamera;

            string layerName = "";

            if (CanvasType == ECanvasType.Overlay) layerName = UIOverlayLayerName;
            else layerName = UILayerName;

            ThisCanvas.sortingLayerName = layerName;
            ThisCanvas.sortingOrder = 0;
        }

        if (UseSafeArea)
        {
            SetupSafeArea();
        }

        OnInitialize();
    }

    private void SetupSafeArea()
    {
        if (_safeAreaRect == null)
        {
            var safeAreaObj = new GameObject("SafeArea");
            safeAreaObj.transform.SetParent(transform, false);
            _safeAreaRect = safeAreaObj.AddComponent<RectTransform>();

            // SafeArea RectTransform 초기 설정
            _safeAreaRect.anchorMin = Vector2.zero;
            _safeAreaRect.anchorMax = Vector2.one;
            _safeAreaRect.offsetMin = Vector2.zero;
            _safeAreaRect.offsetMax = Vector2.zero;

            // 기존 자식들을 SafeArea로 이동
            var children = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child != _safeAreaRect)
                {
                    children.Add(child);
                }
            }

            foreach (var child in children)
            {
                child.SetParent(_safeAreaRect, false);
            }
        }

        ApplySafeArea();
    }

    private void Update()
    {
        if (UseSafeArea && _safeAreaRect != null && (_lastScreenSize.x != Screen.width || _lastScreenSize.y != Screen.height))
        {
            ApplySafeArea();
        }
    }

    private void ApplySafeArea()
    {
        if (_safeAreaRect == null)
        {
            Debug.LogWarning($"[{GetType().Name}] SafeArea RectTransform is null. SafeArea cannot be applied.");
            return;
        }

        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _safeAreaRect.anchorMin = anchorMin;
        _safeAreaRect.anchorMax = anchorMax;

        _lastSafeArea = Screen.safeArea;
        _lastScreenSize = new Vector2Int(Screen.width, Screen.height);
    }

    protected virtual void OnInitialize()
    {
    }

    public virtual ViewCanvas SetVisible(bool flag, bool needAnim = true, bool isOverlay = false,
        bool needForced = false, UnityAction onComplete = null)
    {
        ResetCoroutine(_visibleCoroutine, flag);

        if (flag && ThisCanvas.enabled && needForced.Equals(false))
        {
            return this;
        }
        else if (flag.Equals(false) && ThisCanvas.enabled.Equals(false))
        {
            return this;
        }

        var canvasController = ApplicationManager.Instance.GetModule<CanvasController>();
        if (canvasController == null) return this;

        if (flag)
        {
            if (canvasController.GetTopCanvas() == this)
            {
                return this;
            }

            canvasController.AddToVisibleList(this);

            _onChangeVisible?.Invoke(flag);
            IsVisible = flag;
            ThisCanvas.enabled = flag;
            ThisCanvasGroup.interactable = flag;
            ThisCanvasGroup.blocksRaycasts = flag;

            if (needAnim)
            {
                ThisCanvasGroup.alpha = 0f;
                _visibleCoroutine = StartCoroutine(SetVisibleAnim(0f, 1f));
            }
        }
        else
        {
            if (needAnim)
            {
                ThisCanvasGroup.alpha = 1f;
                _visibleCoroutine = StartCoroutine(SetVisibleAnim(1f, 0f, () => { OffMethod(onComplete); }));
            }
            else
            {
                OffMethod(onComplete);
            }
        }

        return this;
    }

    private void OffMethod(UnityAction onComplete = null)
    {
        IsVisible = false;
        ThisCanvas.enabled = false;
        ThisCanvasGroup.interactable = false;
        ThisCanvasGroup.blocksRaycasts = false;

        var canvasController = ApplicationManager.Instance.GetModule<CanvasController>();
        if (canvasController != null)
        {
            canvasController.RemoveFromVisibleList(this);
        }

        onComplete?.Invoke();
        _onChangeVisible?.Invoke(false);
    }

    private void ResetCoroutine(Coroutine coroutine, bool flag)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
            if (flag)
            {
                ThisCanvasGroup.alpha = 1f;
            }
            else
            {
                ThisCanvasGroup.alpha = 0f;
            }
        }
    }

    private IEnumerator SetVisibleAnim(float start, float end, UnityAction onEnd = null)
    {
        if (ThisCanvasGroup == null)
        {
            yield break;
        }

        var easeFunc = CustomEasing.GetEasingFunction(CustomEasing.Ease.EaseInSine);
        var duration = 0.1f;

        var timer = 0f;

        while (timer < duration)
        {
            var rate = easeFunc.Invoke(0f, 1f, timer / duration);

            var targetAlpha = Mathf.Lerp(start, end, rate);
            ThisCanvasGroup.alpha = targetAlpha;

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        ThisCanvasGroup.alpha = end;

        onEnd?.Invoke();
    }

    private void Dispose()
    {
        _onChangeVisible = null;

        if (_visibleCoroutine != null)
        {
            StopCoroutine(_visibleCoroutine);
            _visibleCoroutine = null;
        }

        if (ThisCanvasGroup != null)
        {
            ThisCanvasGroup.alpha = 0f;
        }

        if (ThisCanvas != null)
        {
            ThisCanvas.enabled = false;
        }

        OnDispose();
    }

    protected virtual void OnDispose()
    {
    }

    public void BindOnChangeVisible(UnityAction<bool> action)
    {
        _onChangeVisible += action;
    }

    public void UnBindOnChangeVisible(UnityAction<bool> action)
    {
        _onChangeVisible -= action;
    }
}