using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public event Action OnShoot;

    [SerializeField] private float _mouseSensitivity = 2.0f;
    [SerializeField] private float _mouseWheelSensitivity = 5.0f;

    private float _mouseX;
    private float _mouseY;
    private float _mouseWheelScroll;

    public float MouseX { get => _mouseX; }
    public float MouseY { get => _mouseY; }
    public float MouseWheelScroll { get => _mouseWheelScroll; }

    private void Awake()
    {
        ServiceLocator.RegisterService<InputController>(this);
    }

    private void Update()
    {
        _mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        _mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        if (Input.GetMouseButtonDown(0))
        {
            OnShoot?.Invoke();
        }

        _mouseWheelScroll = Input.mouseScrollDelta.y * _mouseWheelSensitivity;
    }
}