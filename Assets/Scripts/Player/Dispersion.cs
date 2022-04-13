using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player_InputHandle))]
public class Dispersion : MonoBehaviour
{
    public Action<float, float> OnSetCrosshairValues;
    public Action<float> OnSetAlpha,
        OnSetScale;

    [Range(0, 30.0f)] public float m_ShootDispersion;
    [Range(0, 30.0f)] public float m_DefaultDispersion;
    [Range(0, 30.0f)] public float m_AimDispersion;

    //TODO: per shoot dispersion
    //[Range(0, 30.0f)] public float m_PerShotAddDispersion;
    [Range(0, 30.0f)] public float m_MovementAddDispersion;

    public float m_AimSpeed;
    public float m_ShootSpeed;
    public float m_RecoverSpeed;

    public float m_CurrentDispersion;
    private float m_TargetDispersion;
    private float m_CurrentSpeed;
    private bool m_MaxScale;
    private bool m_StartedMoving;

    private Player_ShootSystem m_ShootSystem;
    private Player_InputHandle m_Input;

    void Awake()
    {
        m_ShootSystem = GetComponent<Player_ShootSystem>();
        m_Input = GetComponent<Player_InputHandle>();
    }
    private void Start()
    {
        OnSetCrosshairValues?.Invoke(m_ShootDispersion, m_AimDispersion);
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
                m_CurrentSpeed = m_RecoverSpeed;
                m_CurrentDispersion = m_TargetDispersion;
                m_TargetDispersion = m_AimDispersion;
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
                m_TargetDispersion += m_MovementAddDispersion;
                m_StartedMoving = true;
            }
        }
        else
        {
            if (m_StartedMoving)
            {
                m_TargetDispersion -= m_MovementAddDispersion;
                m_StartedMoving = false;
            }
        }
        
    }
    private void Shoot()
    {
        m_CurrentSpeed = m_ShootSpeed;
        m_TargetDispersion = m_ShootDispersion;
        m_MaxScale = true;
    }
    private void StartAiming()
    {
        m_CurrentSpeed = m_AimSpeed;
        m_TargetDispersion = m_AimDispersion;
        m_CurrentDispersion = m_DefaultDispersion;
        OnSetAlpha?.Invoke(1.0f);
    }
    private void StopAiming()
    {
        m_TargetDispersion = m_DefaultDispersion;
        OnSetAlpha?.Invoke(0.0f);
    }
}
