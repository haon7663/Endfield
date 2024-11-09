using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ArtDirectionManager : Singleton<ArtDirectionManager>
{
    [Header("Hit")]
    [SerializeField] private Volume dangerVolume;
    [SerializeField] private Volume hitVolume;
    
    [Header("BulletTime")]
    public bool onBulletTime;
    
    [SerializeField] private Volume globalVolume;
    [SerializeField] private Volume bulletTimeVolume;
    [SerializeField] private AnimationCurve bulletTimeVolumeCurve;

    [SerializeField] private int highlightLayerNumber;
    [SerializeField] private Material highlightMaterial;

    private List<Unit> _prevUnits;
    private Dictionary<Unit, int> _prevLayerNumbers;
    private Dictionary<Unit, Material> _prevMaterials;

    public void OnHit()
    {
        DOVirtual.Float(1, 0, 0.6f, value => hitVolume.weight = value).SetEase(Ease.InCubic);
    }
    
    public void EnterDanger()
    {
        DOTween.Kill("DangerSequence");
        DOVirtual.Float(dangerVolume.weight, 1, 0.1f, value => dangerVolume.weight = value).SetEase(Ease.InCubic).SetId("DangerSequence");
    }
    public void ExitDanger()
    {
        DOVirtual.Float(dangerVolume.weight, 0, 0.25f, value => dangerVolume.weight = value).SetEase(Ease.OutCirc).SetId("DangerSequence");
    }

    public void KillUnitTime()
    {
        DOTween.Kill("KillUnitTime", true);
        DOVirtual.Float(TimeCaster.TimeScale, 0.1f, 0.05f, value => TimeCaster.TimeScale = value).OnComplete(() =>
        {
            DOVirtual.Float(TimeCaster.TimeScale, 1f, 0.2f, value => TimeCaster.TimeScale = value);
        }).SetId("KillUnitTime");
    }
    
    public void StartBulletTime(List<Unit> targetUnits = null)
    {
        onBulletTime = true;
        
        SetVolume(true);

        _prevUnits = new List<Unit>();
        _prevLayerNumbers = new Dictionary<Unit, int>();
        _prevMaterials = new Dictionary<Unit, Material>();

        targetUnits?.ForEach(unit =>
        {
            _prevUnits.Add(unit);
            _prevLayerNumbers.Add(unit, unit.Renderer.gameObject.layer);
            _prevMaterials.Add(unit, Sprite2DMaterial.GetDefaultMaterial());
            
            //unit.Renderer.gameObject.layer = highlightLayerNumber;
            unit.Renderer.material = highlightMaterial;
        });
    }

    public void EndBulletTime()
    {
        return;
        
        onBulletTime = false;
        
        SetVolume(false);

        if (_prevUnits == null)
            return;
        foreach (var unit in _prevUnits)
        {
            unit.Renderer.gameObject.layer = _prevLayerNumbers[unit];
            unit.Renderer.material = _prevMaterials[unit];
        }
    }

    private void SetVolume(bool inSimulation)
    {
        DOTween.Kill(this);

        DOVirtual.Float(TimeCaster.TimeScale, inSimulation ? 0.25f : 1, 0.1f, value => TimeCaster.TimeScale = value);
        DOVirtual.Float(bulletTimeVolume.weight, inSimulation ? 1 : 0, 0.1f, value => bulletTimeVolume.weight = value).SetEase(bulletTimeVolumeCurve);
    }
}
