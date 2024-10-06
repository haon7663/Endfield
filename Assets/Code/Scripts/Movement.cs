using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Unit _unit;

    private void Start()
    {
        _unit = GetComponent<Unit>();
    }
}
