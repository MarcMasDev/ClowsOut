using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public event Action<float> OnCameraYawDelta,
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
        OnStartDashing,
        OnStartInteracting,
        OnStartReloading,
        OnStartBacking,
        OnRotatingClockwise,
        OnRotatingCounterClockwise;

    private PlayerInput m_PlayerInput;

    //private static InputManager m_Instance = null;

    //public static InputManager Instance
    //{
    //    get
    //    {
    //        if (m_Instance == null)
    //        {
    //            m_Instance = GameObject.FindObjectOfType<InputManager>();
    //        }
    //        return m_Instance;
    //    }
    //}

    private void Awake()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
        GameManager.GetManager().SetInputManager(this);
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
    public void OnDash(InputAction.CallbackContext context)
    {
        switch (context)
        {
            case var value when context.started:
                OnStartDashing?.Invoke();
                break;
        }
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        switch (context)
        {
            case var value when context.started:
                OnStartInteracting?.Invoke();
                break;
        }
    }
    public void OnBack(InputAction.CallbackContext context)
    {
        switch (context)
        {
            case var value when context.started:
                OnStartBacking?.Invoke();
                break;
        }
    }
    public void SwitchToMenuActionMap()
    {
        m_PlayerInput.SwitchCurrentActionMap("Menu");
    }
    public void SwitchToPlayerActionMap()
    {
        m_PlayerInput.SwitchCurrentActionMap("Player");
    }
    public void OnRestartGame(InputAction.CallbackContext context)
    {
        switch (context)
        {
            case var value when context.started:
                GameManager.GetManager().GetRestartManager().Restart();
                break;
        }
    }
    public void OnReload(InputAction.CallbackContext context)
    {
        switch (context)
        {
            case var value when context.started:
                OnStartReloading?.Invoke();
                break;
        }
    }

    public void OnRotateClockwise(InputAction.CallbackContext context)
    {
        switch (context)
        {
            case var value when context.started:
                OnRotatingClockwise?.Invoke();
                break;
        }
    }
    public void OnRotateCounterClockwise(InputAction.CallbackContext context)
    {
        switch (context)
        {
            case var value when context.started:
                OnRotatingCounterClockwise?.Invoke();
                break;
        }
    }

    public void OnCameraDelta(InputAction.CallbackContext context)
    {
        Vector2 l_CameraDelta = context.ReadValue<Vector2>();
        OnCameraPitchDelta?.Invoke(l_CameraDelta.x);
        OnCameraYawDelta?.Invoke(l_CameraDelta.y);
    }
}
