using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairShader : MonoBehaviour
{
    public Material m_CrosshairMaterial;

    public float m_MaxScale;
    public float m_MinScale;
    public float m_UpdateTime;

    private float m_DispersionRange;
    private float m_MaxDispersion;
    private float m_MinDispersion;
    [SerializeField] private Player_Dispersion m_PlayerDispersion;
    private float m_ScaleRange => m_MaxScale - m_MinScale;
    private bool m_SetScale;
    private float m_CurrentScale;

    private float m_UpdateTimer;

    private void OnEnable()
    {
        m_PlayerDispersion.OnSetDispersionValues += SetDispersionValues;
        m_PlayerDispersion.OnSetScale += SetScale;
    }

    private void OnDisable()
    {
        m_PlayerDispersion.OnSetDispersionValues -= SetDispersionValues;
        m_PlayerDispersion.OnSetScale -= SetScale;
    }
    private void Update()
    {
        if (m_UpdateTimer >= m_UpdateTime)
        {
            if (m_SetScale)
            {
                m_CrosshairMaterial.SetFloat("Vector1_707d150ccf4e470db716e1a55b17515d", m_CurrentScale);
                m_SetScale = false;
                m_UpdateTimer = 0.0f;
            }
        }
        m_UpdateTimer += Time.deltaTime;
    }
    private void SetDispersionValues(float maxDispersion, float minDispersion)
    {
        m_MaxDispersion = maxDispersion;
        m_MinDispersion = minDispersion;
        m_DispersionRange = m_MaxDispersion - m_MinDispersion;
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
}
