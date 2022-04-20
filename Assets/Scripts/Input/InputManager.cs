using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event Action<int> OnCameraYawDelta,
        OnCameraPitchDelta;

    public event Action OnResetMove,
        OnMoveLeft,
        OnMoveRight,
        OnMoveUp,
        OnMoveDown,
        OnStopMoving,
        OnStartShooting,
        OnStopShooting,
        OnStartAiming,
        OnStopAiming,
        OnStartSprinting,
        OnStopSprinting;

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
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 l_MovementAxis = context.ReadValue<Vector2>();

        if (l_MovementAxis == Vector2.zero)
        {
            OnStopMoving?.Invoke();
        }
        else
        {
            OnResetMove?.Invoke();
            if (l_MovementAxis.y > 0)
            {
                OnMoveUp?.Invoke();
            }
            else if (l_MovementAxis.y < 0)
            {
                OnMoveDown?.Invoke();
            }
            if (l_MovementAxis.x > 0)
            {
                OnMoveRight?.Invoke();
            }
            else if (l_MovementAxis.x < 0)
            {
                OnMoveLeft?.Invoke();
            }
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
    public void OnSprint(InputAction.CallbackContext context)
    {
        switch (context)
        {
            case var value when context.started:
                OnStartSprinting?.Invoke();
                break;
            case var value when context.canceled:
                OnStopSprinting?.Invoke();
                break;
        }
    }
}
