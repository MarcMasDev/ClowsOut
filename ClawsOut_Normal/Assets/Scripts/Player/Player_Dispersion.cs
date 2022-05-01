using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player_InputHandle))]
public class Player_Dispersion : MonoBehaviour
{
    public Action<float, float> OnSetCrosshairValues;
    public Action<float> OnSetAlpha,
        OnSetScale;
    public Action<bool> OnSetScaleGO;

    //TODO: per shoot dispersion
    //[Range(0, 30.0f)] public float m_PerShotAddDispersion;

    [HideInInspector] public float m_CurrentDispersion;
    private float m_TargetDispersion;
    private float m_CurrentSpeed;
    private bool m_MaxScale;
    private bool m_StartedMoving;

    private Player_ShootSystem m_ShootSystem;
    private Player_InputHandle m_Input;
    private Player_Blackboard m_Blackboard;

    void Awake()
    {
        m_ShootSystem = GetComponent<Player_ShootSystem>();
        m_Input = GetComponent<Player_InputHandle>();
        m_Blackboard = GetComponent<Player_Blackboard>();
    }
    private void Start()
    {
        OnSetCrosshairValues?.Invoke(m_Blackboard.m_ShootDispersion, m_Blackboard.m_AimDispersion);
        m_TargetDispersion = m_Blackboard.m_DefaultDispersion;
        m_CurrentDispersion = m_Blackboard.m_DefaultDispersion;
        m_CurrentSpeed = m_Blackboard.m_DefaultSpeed;
        OnSetScaleGO?.Invoke(false);
        ShowCrosshair();
    }
    private void OnEnable()
    {
        m_ShootSystem.OnShoot += Shoot;
        InputManager.Instance.OnStartAiming += StartAiming;
        InputManager.Instance.OnStopAiming += StopAiming;
    }
    private void OnDisable()
    {
        m_ShootSystem.OnShoot -= Shoot;
        InputManager.Instance.OnStartAiming -= StartAiming;
        InputManager.Instance.OnStopAiming -= StopAiming;
    }

    void Update()
    {
        AddedDispersion();
        m_CurrentDispersion = Mathf.Lerp(m_CurrentDispersion, m_TargetDispersion, m_CurrentSpeed * Time.deltaTime);

        if (m_MaxScale)
        {
            if (m_CurrentDispersion >= m_TargetDispersion - m_TargetDispersion * 0.05f)
            {
                m_CurrentSpeed = m_Blackboard.m_RecoverSpeed;
                m_CurrentDispersion = m_TargetDispersion;
                if (m_Input.Aiming)
                {
                    m_TargetDispersion = m_Blackboard.m_AimDispersion;
                }
                else
                {
                    m_TargetDispersion = m_Blackboard.m_DefaultDispersion;
                }
                m_MaxScale = false;
            }
        }
        OnSetScale?.Invoke(m_CurrentDispersion);
    }
    private void AddedDispersion()
    {
        if (m_Input.Moving)
        {
            if (!m_StartedMoving)
            {
                m_TargetDispersion += m_Blackboard.m_MovementAddDispersion;
                m_StartedMoving = true;
            }
        }
        else
        {
            if (m_StartedMoving)
            {
                m_TargetDispersion -= m_Blackboard.m_MovementAddDispersion;
                m_StartedMoving = false;
            }
        }
        
    }
    private void Shoot()
    {
        m_CurrentSpeed = m_Blackboard.m_ShootSpeed;
        m_TargetDispersion = m_Blackboard.m_ShootDispersion;
        m_MaxScale = true;
    }
    private void StartAiming()
    {
        m_CurrentSpeed = m_Blackboard.m_AimSpeed;
        m_TargetDispersion = m_Blackboard.m_AimDispersion;
        OnSetScaleGO?.Invoke(true);
    }
    private void StopAiming()
    {
        m_TargetDispersion = m_Blackboard.m_DefaultDispersion;
        m_CurrentSpeed = m_Blackboard.m_DefaultSpeed;
        OnSetScaleGO?.Invoke(false);
    }
    public void ShowCrosshair()
    {
        OnSetAlpha?.Invoke(1.0f);
    }
    public void HideCrosshair()
    {
        OnSetAlpha?.Invoke(0.0f);
    }
}
