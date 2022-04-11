using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Material m_CrosshairMaterial;

    public float m_ShootSpeed;
    public float m_AimSpeed;
    public float m_RecoverSpeed;
    public float m_DefaultScale;
    public float m_ShootScale;
    public float m_AimScale;

    private float m_CurrentSpeed;
    private float m_CurrentScale;
    private float m_CurrentAlpha;
    private float m_TargetScale;
    private float m_TargetAlpha;
    private bool m_MaxScale;

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
        m_TargetScale = m_DefaultScale;
        m_CurrentScale = m_DefaultScale;
        m_CurrentAlpha = 0.0f;
    }
    private void Update()
    {
        m_CurrentScale = Mathf.Lerp(m_CurrentScale, m_TargetScale, m_CurrentSpeed * Time.deltaTime);
        m_CurrentAlpha = Mathf.Lerp(m_TargetAlpha, m_CurrentAlpha, m_CurrentSpeed * Time.deltaTime);

        if (m_MaxScale)
        {
            if (m_CurrentScale >= m_TargetScale - m_TargetScale * 0.05f)
            {
                m_CurrentSpeed = m_RecoverSpeed;
                m_CurrentScale = m_TargetScale;
                m_TargetScale = m_AimScale;
                m_MaxScale = false;
            }
        }

        m_CrosshairMaterial.SetFloat("Vector1_707d150ccf4e470db716e1a55b17515d", m_CurrentScale);
        m_CrosshairMaterial.SetFloat("Vector1_95c6165cb268478aab16ec2165a50b11", m_CurrentAlpha);
    }
    private void Shoot()
    {
        m_TargetScale = m_ShootScale;
        m_CurrentSpeed = m_ShootSpeed;
        m_MaxScale = true;

    }
    private void StartAim()
    {
        m_TargetScale = m_AimScale;
        m_CurrentSpeed = m_AimSpeed;
        m_TargetAlpha = 1.0f;
    }
    private void StopAim()
    {
        m_TargetScale = m_DefaultScale;
        m_CurrentScale = m_DefaultScale;
        m_TargetAlpha = 0.0f;
    }
}
