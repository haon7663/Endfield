using System;
using UnityEngine;
using UnityEngine.UIElements;
using SandyToolkitCore;
using SandyCore;

public abstract class UIDocumentView : MonoBehaviour
{
    [field: SerializeField] public bool IsCloseable { get; protected set; }
    [SerializeField] private bool _isOverlay;

    public UIDocument ThisUIDocument { get; protected set; }
    public VisualElement RootElement { get; protected set; }
    public bool IsVisible { get; protected set; }

    protected virtual void OnDestroy()
    {
        if (!_isOverlay)
        {
            if (ApplicationManager.Instance != null)
            {
                var uiDocumentController = ApplicationManager.Instance.GetModule<UIDocumentController>();
                if (uiDocumentController != null)
                {
                    uiDocumentController.UnregisterView(this);
                    uiDocumentController.RemoveFromVisibleList(this);
                }
            }
            Dispose();
        }
    }

    public virtual void Setup()
    {
        if (ThisUIDocument == null)
            ThisUIDocument = GetComponent<UIDocument>();

        if (RootElement == null)
            RootElement = ThisUIDocument.rootVisualElement;

        RootElement.style.display = DisplayStyle.None;
        RootElement.schedule.Execute(() =>
        {
            RootElement.style.display = DisplayStyle.Flex;
        });

        OnInitialize();
    }

    protected virtual void OnInitialize()   
    {
    }

    public virtual UIDocumentView SetVisible(bool flag, bool needAnim = true, bool isOverlay = false)
    {
        if (flag && RootElement.style.display == DisplayStyle.Flex)
        {
            return this;
        }
        else if (!flag && RootElement.style.display == DisplayStyle.None)
        {
            return this;
        }

        var uiDocumentController = ApplicationManager.Instance.GetModule<UIDocumentController>();
        if (uiDocumentController == null) return this;

        if (flag)
        {
            if (uiDocumentController.GetTopView() == this)
            {
                return this;
            }

            uiDocumentController.AddToVisibleList(this, isOverlay);
            IsVisible = flag;
            RootElement.style.display = DisplayStyle.Flex;
        }
        else
        {
            IsVisible = false;
            RootElement.style.display = DisplayStyle.None;

            var controller = ApplicationManager.Instance.GetModule<UIDocumentController>();
            if (controller != null)
            {
                controller.RemoveFromVisibleList(this);
            }
        }

        return this;
    }

    protected void Dispose()
    {
        if (RootElement != null)
        {
            RootElement.style.display = DisplayStyle.None;
        }

        OnDispose();
    }

    protected virtual void OnDispose()
    {
    }
}