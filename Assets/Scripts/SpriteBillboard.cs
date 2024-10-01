using System;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField] private bool freezeXZAxis = true;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        transform.rotation = freezeXZAxis
            ? Quaternion.Euler(0f, _camera.transform.rotation.eulerAngles.y, 0f)
            : _camera.transform.rotation;
    }
}
