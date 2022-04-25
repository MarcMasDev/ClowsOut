using System;
using UnityEngine;

public class Player_InputHandle : MonoBehaviour
{
    private Vector2 m_MovementAxis;
    public Vector2 MovementAxis { get { return m_MovementAxis.normalized; } private set { m_MovementAxis = value; } }
    public bool Moving { get; private set; }
    public bool Shooting { get; private set; }
    public bool Aiming { get; private set; }
    public bool Dashing { get; set; }

    private void OnEnable()
    {
        InputManager.Instance.OnResetMove += ResetMove;
        InputManager.Instance.OnMoveLeft += MoveLeft;
        InputManager.Instance.OnMoveRight += MoveRight;
        InputManager.Instance.OnMoveUp += MoveUp;
        InputManager.Instance.OnMoveDown += MoveDown;
        InputManager.Instance.OnStopMoving += StopMoving;
        InputManager.Instance.OnStartShooting += StartShooting;
        InputManager.Instance.OnStopShooting += StopShooting;
        InputManager.Instance.OnStartAiming += StartAiming;
        InputManager.Instance.OnStopAiming += StopAiming;
        InputManager.Instance.OnStartDashing += StartDashing;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnResetMove -= ResetMove;
        InputManager.Instance.OnMoveLeft -= MoveLeft;
        InputManager.Instance.OnMoveRight -= MoveRight;
        InputManager.Instance.OnMoveUp -= MoveUp;
        InputManager.Instance.OnMoveDown -= MoveDown;
        InputManager.Instance.OnStopMoving -= StopMoving;
        InputManager.Instance.OnStartShooting -= StartShooting;
        InputManager.Instance.OnStopShooting -= StopShooting;
        InputManager.Instance.OnStartAiming -= StartAiming;
        InputManager.Instance.OnStopAiming -= StopAiming;
        InputManager.Instance.OnStartDashing -= StartDashing;
    }

    private void ResetMove()
    {
        m_MovementAxis = Vector2.zero;
    }
    private void MoveLeft()
    {
        m_MovementAxis += new Vector2(-1, 0);
        Moving = true;
    }

    private void MoveRight()
    {
        m_MovementAxis += new Vector2(1, 0);
        Moving = true;
    }

    private void MoveUp()
    {
        m_MovementAxis += new Vector2(0, 1);
        Moving = true;
    }

    private void MoveDown()
    {
        m_MovementAxis += new Vector2(0, -1);
        Moving = true;
    }

    private void StopMoving()
    {
        m_MovementAxis = Vector2.zero;
        Moving = false;
    }

    private void StartShooting()
    {
        Shooting = true;
    }
    private void StopShooting()
    {
        Shooting = false;
    }

    private void StartAiming()
    {
        Aiming = true;
    }

    public void StopAiming()
    {
        Aiming = false;
    }
    private void StartDashing()
    {
        Dashing = true;
    }
}
