using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player_InputHandle))]
public class Player_Dispersion : MonoBehaviour
{
    public Action<float, float> OnSetDispersionValues;
    public Action<float> OnSetScale;

    //TODO: per shoot dispersion
    //[Range(0, 30.0f)] public float m_PerShotAddDispersion;

    [HideInInspector] public float m_CurrentDispersion;
    private float m_TargetDispersion;
    private float m_CurrentSpeed;
    private bool m_Shooted;
    private bool m_StartedMoving;

    private Player_ShootSystem m_ShootSystem;
    private Player_InputHandle m_Input;
    private Player_Blackboard m_Blackboard;
    private float m_AddedMovementDispersion;
    private bool m_Started;

    void Awake()
    {
        m_ShootSystem = GetComponent<Player_ShootSystem>();
        m_Input = GetComponent<Player_InputHandle>();
        m_Blackboard = GetComponent<Player_Blackboard>();
    }
    private void Start()
    {
        OnSetDispersionValues?.Invoke(m_Blackboard.m_ShootDispersion, m_Blackboard.m_AimDispersion);
        m_TargetDispersion = m_Blackboard.m_DefaultDispersion;
        m_AddedMovementDispersion = 0;
        m_CurrentDispersion = m_Blackboard.m_DefaultDispersion;
        m_CurrentSpeed = m_Blackboard.m_DefaultSpeed;
    }
    private void OnEnable()
    {
        m_ShootSystem.OnShoot += Shoot;
    }
    private void OnDisable()
    {
        m_ShootSystem.OnShoot -= Shoot;
    }

    void Update()
    {
        AddedDispersion();
        m_CurrentDispersion = Mathf.Lerp(m_CurrentDispersion, m_TargetDispersion + m_AddedMovementDispersion, m_CurrentSpeed * Time.deltaTime);

        if (m_Shooted)
        {
            if (m_CurrentDispersion >= (m_TargetDispersion + m_AddedMovementDispersion) - (m_TargetDispersion + m_AddedMovementDispersion) * 0.05f)
            {
                Debug.Log("Done");
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
                m_Shooted = false;
            }
        }
        else
        {
            if (m_Input.Aiming && !m_Started)
            {
                StartAiming();
                m_Started = true;
            }
            else if (!m_Input.Aiming)
            {
                StopAiming();
                m_Started = false;
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
                m_AddedMovementDispersion = m_Blackboard.m_MovementAddDispersion;
                m_StartedMoving = true;
            }
        }
        else
        {
            if (m_StartedMoving)
            {
                m_AddedMovementDispersion = 0;
                m_StartedMoving = false;
            }
        }

    }
    private void Shoot()
    {
        m_CurrentSpeed = m_Blackboard.m_ShootSpeed;
        m_TargetDispersion = m_Blackboard.m_ShootDispersion;
        m_Shooted = true;
    }
    private void StartAiming()
    {
        m_CurrentSpeed = m_Blackboard.m_AimSpeed;
        m_TargetDispersion = m_Blackboard.m_AimDispersion;
    }
    private void StopAiming()
    {
        m_TargetDispersion = m_Blackboard.m_DefaultDispersion;
        m_CurrentSpeed = m_Blackboard.m_DefaultSpeed;
    }
}
