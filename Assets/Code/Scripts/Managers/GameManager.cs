using System;
using UnityEngine;

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
