using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Material m_CrosshairMaterial;

    public float m_MaxScale;
    public float m_MinScale;
    public float m_DisappearSpeed;
    public Player_Dispersion m_Dispersion;

    private float m_CurrentAlpha;
    private float m_TargetAlpha;

    private float m_DispersionRange;
    private float m_MaxDispersion;
    private float m_MinDispersion;
    private float m_ScaleRange => m_MaxScale - m_MinScale;

    private void Awake()
    {
        m_TargetAlpha = 0.0f;
        m_CurrentAlpha = m_TargetAlpha;
    }
    private void OnEnable()
    {
        m_Dispersion.OnSetCrosshairValues += SetCrosshairValues;
        m_Dispersion.OnSetScale += SetScale;
        m_Dispersion.OnSetAlpha += SetAlpha;
    }

    private void OnDisable()
    {
        m_Dispersion.OnSetCrosshairValues -= SetCrosshairValues;
        m_Dispersion.OnSetScale -= SetScale;
        m_Dispersion.OnSetAlpha -= SetAlpha;
    }
    private void Update()
    {
        if (m_CurrentAlpha != m_TargetAlpha)
        {
            m_CurrentAlpha = Mathf.Lerp(m_CurrentAlpha, m_TargetAlpha, m_DisappearSpeed * Time.deltaTime);
            m_CrosshairMaterial.SetFloat("Vector1_95c6165cb268478aab16ec2165a50b11", m_CurrentAlpha);
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
        scale = (m_MaxScale + m_MinScale) - ((((scale - m_MinDispersion) * m_ScaleRange)/ m_DispersionRange) + m_MinScale);
        m_CrosshairMaterial.SetFloat("Vector1_707d150ccf4e470db716e1a55b17515d", scale);
    }
}
