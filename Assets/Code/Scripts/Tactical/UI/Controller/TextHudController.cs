using System;
using UnityEngine;

public class TextHudController : Singleton<TextHudController>
{
    [SerializeField] private TextHud textHudPrefab;
    [SerializeField] private Transform canvas;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public void Show(Vector3 pos, string text)
    {
        var textHud = Instantiate(textHudPrefab, canvas);
        textHud.transform.position = _mainCamera.WorldToScreenPoint(pos);
        textHud.Init(text);
    }
}
