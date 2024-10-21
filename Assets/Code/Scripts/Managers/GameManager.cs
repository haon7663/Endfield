using DG.Tweening;
using NUnit.Framework;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : Singleton<GameManager>
{
    public Unit player;

    public int maxElixir;
    public float curElixir;

    private void Update()
    {
        curElixir += Time.deltaTime;
        curElixir = Mathf.Clamp(curElixir, 0, maxElixir);
    }
}
