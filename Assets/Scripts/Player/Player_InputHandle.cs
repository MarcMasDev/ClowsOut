using System;
using UnityEngine;

public class Player_InputHandle : MonoBehaviour
{
    private Vector2 m_MovementAxis;
    public Vector2 MovementAxis { get { return m_MovementAxis.normalized; } private set { m_MovementAxis = value; } }
    public bool Moving { get; private set; }
    public bool Shooting { get; set; }
    public bool Aiming { get; private set; }
    public bool Dashing { get; set; }
    public bool Reloading { get; set; }
    private void OnEnable()
    {
        GameManager.GetManager().GetInputManager().OnResetMove += ResetMove;
        GameManager.GetManager().GetInputManager().OnMoveLeft += MoveLeft;
        GameManager.GetManager().GetInputManager().OnMoveRight += MoveRight;
        GameManager.GetManager().GetInputManager().OnMoveUp += MoveUp;
        GameManager.GetManager().GetInputManager().OnMoveDown += MoveDown;
        GameManager.GetManager().GetInputManager().OnStopMoving += StopMoving;
        GameManager.GetManager().GetInputManager().OnStartShooting += StartShooting;
        GameManager.GetManager().GetInputManager().OnStopShooting += StopShooting;
        GameManager.GetManager().GetInputManager().OnStartAiming += StartAiming;
        GameManager.GetManager().GetInputManager().OnStopAiming += StopAiming;
        GameManager.GetManager().GetInputManager().OnStartDashing += StartDashing;
        GameManager.GetManager().GetInputManager().OnStartReloading += StartReloading;
    }

    private void OnDisable()
    {
        GameManager.GetManager().GetInputManager().OnResetMove -= ResetMove;
        GameManager.GetManager().GetInputManager().OnMoveLeft -= MoveLeft;
        GameManager.GetManager().GetInputManager().OnMoveRight -= MoveRight;
        GameManager.GetManager().GetInputManager().OnMoveUp -= MoveUp;
        GameManager.GetManager().GetInputManager().OnMoveDown -= MoveDown;
        GameManager.GetManager().GetInputManager().OnStopMoving -= StopMoving;
        GameManager.GetManager().GetInputManager().OnStartShooting -= StartShooting;
        GameManager.GetManager().GetInputManager().OnStopShooting -= StopShooting;
        GameManager.GetManager().GetInputManager().OnStartAiming -= StartAiming;
        GameManager.GetManager().GetInputManager().OnStopAiming -= StopAiming;
        GameManager.GetManager().GetInputManager().OnStartDashing -= StartDashing;
    }
    private void OnApplicationQuit()
    {
        MonoBehaviour[] scripts = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
            script.enabled = false;
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
    public void StartReloading()
    {
        Reloading = true;
    }
}
