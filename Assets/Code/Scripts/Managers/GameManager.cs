using NUnit.Framework;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : Singleton<GameManager>
{
    public Unit player;
    [SerializeField] private Volume globalVolume;
    [SerializeField] private ColorAdjustments colorAdjustments;

    public int maxElixir;
    public float curElixir;
    private void Start()
    {
        if (globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments)) { }
    }

    private void Update()
    {
        curElixir += Time.deltaTime;
        curElixir = Mathf.Clamp(curElixir, 0, maxElixir);
    }

    public void BSVolume(bool inSimulation)
    {  
        colorAdjustments.saturation.value = inSimulation? -100:0;
    }
}
