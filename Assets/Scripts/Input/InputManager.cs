using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event Action<int> OnCameraYawDelta,
        OnCameraPitchDelta;

    public event Action OnMoveLeft,
        OnMoveRight,
        OnMoveUp,
        OnMoveDown,
        OnStopMoving,
        OnStartShooting,
        OnStopShooting,
        OnStartAiming,
        OnStopAiming;

    public static InputManager m_instance = null;

    public static InputManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<InputManager>();
            }
            return m_instance;
        }
    }
    protected void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else if (m_instance != this)
        {
            Destroy(this);
        }
    }

    public void OnCameraYawRotate(InputAction input)
    {
        OnCameraYawDelta.Invoke(input.ReadValue<int>());
    }
    public void OnCameraPitchRotate(InputAction input)
    {
        OnCameraPitchDelta.Invoke(input.ReadValue<int>());
    }
    //TODO: Camera sensitivity
    public void OnMove(InputAction.CallbackContext context)
    {
        switch (context.ReadValue<Vector2>())
        {
            case var value when value.y > 0:
                OnMoveUp?.Invoke();
                break;
            case var value when value.y < 0:
                OnMoveDown?.Invoke();
                break;
            case var value when value.x > 0:
                OnMoveRight?.Invoke();
                break;
            case var value when value.x < 0:
                OnMoveLeft?.Invoke();
                break;
            case var value when value == Vector2.zero:
                OnStopMoving?.Invoke();
                break;
        }
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        switch (context)
        {
            case var value when context.started:
                OnStartShooting?.Invoke();
                break;
            case var value when context.canceled:
                OnStopShooting?.Invoke();
                break;
        }
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        switch (context)
        {
            case var value when context.started:
                OnStartAiming?.Invoke();
                break;
            case var value when context.canceled:
                OnStopAiming?.Invoke();
                break;
        }
    }
}
