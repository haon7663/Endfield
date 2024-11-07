using System;
using UnityEngine;
using UnityEngine.Serialization;

public class TextHudController : Singleton<TextHudController>
{
    [SerializeField] private TextHud damageHudPrefab;
    [SerializeField] private TextHud recoveryHudPrefab;
    [SerializeField] private TextHud barrierHudPrefab;
    [SerializeField] private TextHud elixirConsumeHudPrefab;
    [SerializeField] private Transform canvas;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public void ShowElixirConsume(Vector3 pos, int skillElixir)
    {
        var textHud = Instantiate(elixirConsumeHudPrefab, canvas);
        textHud.transform.position = _mainCamera.WorldToScreenPoint(pos);
        textHud.Init($"엘릭서가 부족합니다. ({skillElixir - GameManager.Inst.curElixir:N1})");
    }
    
    public void ShowDamage(Vector3 pos, int value)
    {
        var textHud = Instantiate(damageHudPrefab, canvas);
        textHud.transform.position = _mainCamera.WorldToScreenPoint(pos);
        textHud.Init(value.ToString());
    }
    
    public void ShowRecovery(Vector3 pos, int value)
    {
        var textHud = Instantiate(recoveryHudPrefab, canvas);
        textHud.transform.position = _mainCamera.WorldToScreenPoint(pos);
        textHud.Init(value.ToString());
    }
    
    public void ShowBarrier(Vector3 pos, int value)
    {
        var textHud = Instantiate(barrierHudPrefab, canvas);
        textHud.transform.position = _mainCamera.WorldToScreenPoint(pos);
        textHud.Init(value.ToString());
    }
}
