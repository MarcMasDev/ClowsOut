using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Material m_CrosshairMaterial;

    public float m_ShootSpeed;
    public float m_AimSpeed;
    public float m_DefaultScale;
    public float m_ShootScale;
    public float m_AimScale;

    public float m_CurrentSpeed;
    private float m_CurrentScale;
    private float m_CurrentAlpha;
    private float m_TargetScale;
    private float m_TargetAlpha;
    private bool m_Shooting;

    private void OnEnable()
    {
        InputManager.Instance.OnStartShooting += Shoot;
        InputManager.Instance.OnStartAiming += StartAim;
        InputManager.Instance.OnStopAiming += StopAim;
    }
    private void OnDisable()
    {
        InputManager.Instance.OnStartShooting -= Shoot;
        InputManager.Instance.OnStartAiming -= StartAim;
        InputManager.Instance.OnStopAiming -= StopAim;
    }
    private void Start()
    {
        m_CurrentScale = m_DefaultScale;
        m_CurrentAlpha = 0.0f;
    }
    private void Update()
    {
        m_CurrentScale = Mathf.Lerp(m_CurrentScale, m_TargetScale, m_CurrentSpeed * Time.deltaTime);
        m_CurrentAlpha = Mathf.Lerp(m_TargetAlpha, m_CurrentAlpha, m_CurrentSpeed * Time.deltaTime);

        if (m_Shooting)
        {
            if (m_CurrentScale >= m_TargetScale - m_TargetScale * 0.05f)
            {
                m_CurrentSpeed = m_AimSpeed;
                m_CurrentScale = m_TargetScale;
                m_TargetScale = m_AimScale;
                m_Shooting = false;
            }
        }

        transform.localScale = new Vector3(m_CurrentScale, m_CurrentScale, m_CurrentScale);
        m_CrosshairMaterial.SetFloat("Vector1_742d282584d0402195187c0f91a16d48", m_CurrentAlpha);
    }
    private void Shoot()
    {
        m_TargetScale = m_ShootScale;
        m_CurrentSpeed = m_ShootSpeed;
        m_Shooting = true;

    }
    private void StartAim()
    {
        m_TargetScale = m_AimScale;
        m_CurrentSpeed = m_AimSpeed;
        m_TargetAlpha = 1.0f;
    }
    private void StopAim()
    {
        m_CurrentScale = m_DefaultScale;
        m_TargetAlpha = 0.0f;
    }
}
