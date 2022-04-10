using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event Action<Vector2> OnCameraMoveDelta;
    public event Action OnMoveLeft,
        OnMoveRight,
        OnMoveUp,
        OnMoveDown,
        OnStopMoving,
        OnStartShooting,
        OnStopShooting,
        OnStartAiming,
        OnStopAiming;

    public void OnCameraMove(InputAction.CallbackContext context)
    {
        OnCameraMoveDelta.Invoke(context.ReadValue<Vector2>());
        //TODO: Camera sensitivity
    }
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
