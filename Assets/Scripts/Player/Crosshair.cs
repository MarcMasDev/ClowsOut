using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Material m_CrosshairMaterial;

    public float m_DefaultScaleGO;
    public float m_AimScaleGO;
    public float m_ScaleGOSpeed;
    public float m_MaxScale;
    public float m_MinScale;
    public float m_DisappearSpeed;
    public Player_Dispersion m_Dispersion;

    private float m_CurrentScaleGO;
    private float m_TargetScaleGO;
    private float m_CurrentAlpha;
    private float m_TargetAlpha;

    private float m_DispersionRange;
    private float m_MaxDispersion;
    private float m_MinDispersion;
    private float m_ScaleRange => m_MaxScale - m_MinScale;
    private bool m_SetScale;
    private float m_CurrentScale;

    private void Awake()
    {
        m_TargetAlpha = 1.0f;
        m_CurrentAlpha = m_TargetAlpha;
        m_TargetScaleGO = m_DefaultScaleGO;
        m_CurrentScaleGO = m_TargetScaleGO;
    }
    private void OnEnable()
    {
        m_Dispersion.OnSetCrosshairValues += SetCrosshairValues;
        m_Dispersion.OnSetScale += SetScale;
        m_Dispersion.OnSetScaleGO += SetScaleGO;
        m_Dispersion.OnSetAlpha += SetAlpha;
    }

    private void OnDisable()
    {
        m_Dispersion.OnSetCrosshairValues -= SetCrosshairValues;
        m_Dispersion.OnSetScale -= SetScale;
        m_Dispersion.OnSetScaleGO -= SetScaleGO;
        m_Dispersion.OnSetAlpha -= SetAlpha;
    }
    private void FixedUpdate()
    {
        if (m_CurrentScaleGO != m_TargetScaleGO)
        {
            m_CurrentScaleGO = Mathf.Lerp(m_CurrentScaleGO, m_TargetScaleGO, m_ScaleGOSpeed * Time.deltaTime);
            transform.localScale = new Vector3(m_CurrentScaleGO, m_CurrentScaleGO, m_CurrentScaleGO);
        }
        if (m_CurrentAlpha != m_TargetAlpha)
        {
            m_CurrentAlpha = Mathf.Lerp(m_CurrentAlpha, m_TargetAlpha, m_DisappearSpeed * Time.deltaTime);
            m_CrosshairMaterial.SetFloat("Vector1_95c6165cb268478aab16ec2165a50b11", m_CurrentAlpha);
        }
        if (m_SetScale)
        {
            m_CrosshairMaterial.SetFloat("Vector1_707d150ccf4e470db716e1a55b17515d", m_CurrentScale);
            m_SetScale = false;
        }
    }
    private void SetCrosshairValues(float maxDispersion, float minDispersion)
    {
        m_MaxDispersion = maxDispersion;
        m_MinDispersion = minDispersion;
        m_DispersionRange = m_MaxDispersion - m_MinDispersion;
    }
    private void SetAlpha(float alpha)
    {
        m_TargetAlpha = alpha;
    }
    private void SetScale(float scale)
    {
        m_CurrentScale = (m_MaxScale + m_MinScale) - ((((scale - m_MinDispersion) * m_ScaleRange)/ m_DispersionRange) + m_MinScale);
        if (m_CurrentScale > m_MaxScale)
        {
            m_CurrentScale = m_MaxScale;
        }
        else if (m_CurrentScale < m_MinScale)
        {
            m_CurrentScale = m_MinScale;
        }
        m_SetScale = true;
    }
    private void SetScaleGO(bool aiming)
    {
        if (aiming)
        {
            m_TargetScaleGO = m_AimScaleGO;
        }
        else
        {
            m_TargetScaleGO = m_DefaultScaleGO;
        }
    }
}
