using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ArtDirectionManager : Singleton<ArtDirectionManager>
{
    public bool onBulletTime;
    
    [SerializeField] private Volume globalVolume;
    [SerializeField] private Volume bulletTimeVolume;
    
    private ColorAdjustments _colorAdjustments;
    private ChromaticAberration _chromaticAberration;

    [SerializeField] private int highlightLayerNumber;
    [SerializeField] private Material highlightMaterial;

    private List<Unit> _prevUnits;
    private Dictionary<Unit, int> _prevLayerNumbers;
    private Dictionary<Unit, Material> _prevMaterials;

    private void Awake()
    {
        globalVolume.profile.TryGet(out _colorAdjustments);
        globalVolume.profile.TryGet(out _chromaticAberration);
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
            _prevMaterials.Add(unit, unit.Renderer.material);
            
            //unit.Renderer.gameObject.layer = highlightLayerNumber;
            unit.Renderer.material = highlightMaterial;
        });
    }

    public void EndBulletTime()
    {
        onBulletTime = false;
        
        SetVolume(false);
        
        foreach (var unit in _prevUnits)
        {
            unit.Renderer.gameObject.layer = _prevLayerNumbers[unit];
            unit.Renderer.material = _prevMaterials[unit];
        }
    }

    private void SetVolume(bool inSimulation)
    {
        DOTween.Kill(this);
        TimeCaster.TimeScale = inSimulation ? 0.05f : 1;
        /*DOVirtual.Float(TimeCaster.TimeScale, inSimulation ? 0.02f : 1, 0.1f,
            value => TimeCaster.TimeScale = value);*/

        
        DOVirtual.Float(bulletTimeVolume.weight, inSimulation ? 1 : 0, 0.1f, value => bulletTimeVolume.weight = value).SetUpdate(true);
        /*DOVirtual.Float(_colorAdjustments.postExposure.value, inSimulation ? -1.35f : -1.25f, 0.1f,
            value => _colorAdjustments.postExposure.value = value);
        DOVirtual.Float(_colorAdjustments.saturation.value, inSimulation ? -50 : 0, 0.1f,
            value => _colorAdjustments.saturation.value = value);
        DOVirtual.Float(_chromaticAberration.intensity.value, inSimulation ?  0.15f : 0.05f, 0.1f,
            value => _chromaticAberration.intensity.value = value).SetEase(Ease.OutCirc);*/
        //추가적인 연출
    }
}
