using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public RectTransform[] m_CrosshairRects;
    public Vector3[] m_CrosshairDirections;
    public float m_MinSpread;
    public float m_MaxSpread;
    //TODO: Take from gamecontroller
    public Player_Dispersion m_Dispersion;

    private float m_SpreadRange;
    private float m_TargetSpread;
    private float m_CurrentSpread;
    private float m_MaxDispersion;
    private float m_MinDispersion;
    private float m_DispersionRange;
    private bool m_SetSpread;

    private void OnEnable()
    {
        m_Dispersion.OnSetDispersionValues += SetDispersionValues;
        m_Dispersion.OnSetScale += SetSpread;
    }

    private void OnDisable()
    {
        m_Dispersion.OnSetDispersionValues -= SetDispersionValues;
        m_Dispersion.OnSetScale -= SetSpread;
    }

    private void Awake()
    {
        m_SpreadRange = m_MaxSpread - m_MinSpread;
        m_TargetSpread = m_MinSpread;
        m_CurrentSpread = m_TargetSpread;
    }
    private void Update()
    {
        //if (m_UpdateTimer >= m_UpdateTime)
        //{
            if (m_SetSpread)
            {
                for (int i = 0; i < m_CrosshairRects.Length; i++)
                {
                    m_CrosshairRects[i].localPosition = m_CrosshairDirections[i] * m_CurrentSpread;
                }
                m_SetSpread = false;
                //m_UpdateTimer = 0.0f;
            }
        //}
        //m_UpdateTimer += Time.deltaTime;
    }
    private void SetDispersionValues(float maxDispersion, float minDispersion)
    {
        m_MaxDispersion = maxDispersion;
        m_MinDispersion = minDispersion;
        m_DispersionRange = m_MaxDispersion - m_MinDispersion;
    }
    private void SetSpread(float dispersion)
    {
        m_CurrentSpread = (dispersion - m_MinDispersion) / m_DispersionRange * m_SpreadRange + m_MinSpread;
        if (m_CurrentSpread > m_MaxSpread)
        {
            m_CurrentSpread = m_MaxSpread;
        }
        else if (m_CurrentSpread < m_MinSpread)
        {
            m_CurrentSpread = m_MinSpread;
        }
        m_SetSpread = true;
    }

    //public Material m_CrosshairMaterial;

    //public float m_DefaultScaleGO;
    //public float m_AimScaleGO;
    //public float m_ScaleGOSpeed;
    //public float m_MaxScale;
    //public float m_MinScale;
    //public float m_DisappearSpeed;
    //public Player_Dispersion m_Dispersion;
    //public float m_UpdateTime;

    //private float m_CurrentScaleGO;
    //private float m_TargetScaleGO;
    //private float m_CurrentAlpha;
    //private float m_TargetAlpha;

    //private float m_DispersionRange;
    //private float m_MaxDispersion;
    //private float m_MinDispersion;
    //private float m_ScaleRange => m_MaxScale - m_MinScale;
    //private bool m_SetScale;
    //private float m_CurrentScale;

    //private float m_UpdateTimer;

    //private void Awake()
    //{
    //    m_TargetAlpha = 1.0f;
    //    m_CurrentAlpha = m_TargetAlpha;
    //    m_TargetScaleGO = m_DefaultScaleGO;
    //    m_CurrentScaleGO = m_TargetScaleGO;
    //}
    //private void OnEnable()
    //{
    //    m_Dispersion.OnSetCrosshairValues += SetCrosshairValues;
    //    m_Dispersion.OnSetScale += SetScale;
    //    m_Dispersion.OnSetScaleGO += SetScaleGO;
    //    m_Dispersion.OnSetAlpha += SetAlpha;
    //}

    //private void OnDisable()
    //{
    //    m_Dispersion.OnSetCrosshairValues -= SetCrosshairValues;
    //    m_Dispersion.OnSetScale -= SetScale;
    //    m_Dispersion.OnSetScaleGO -= SetScaleGO;
    //    m_Dispersion.OnSetAlpha -= SetAlpha;
    //}
    //private void Update()
    //{
    //    if (m_CurrentScaleGO != m_TargetScaleGO)
    //    {
    //        m_CurrentScaleGO = Mathf.Lerp(m_CurrentScaleGO, m_TargetScaleGO, m_ScaleGOSpeed * Time.deltaTime);
    //        transform.localScale = new Vector3(m_CurrentScaleGO, m_CurrentScaleGO, m_CurrentScaleGO);
    //    }
    //    if (m_CurrentAlpha != m_TargetAlpha)
    //    {
    //        m_CurrentAlpha = Mathf.Lerp(m_CurrentAlpha, m_TargetAlpha, m_DisappearSpeed * Time.deltaTime);
    //        m_CrosshairMaterial.SetFloat("Vector1_95c6165cb268478aab16ec2165a50b11", m_CurrentAlpha);
    //    }
    //    if (m_UpdateTimer >= m_UpdateTime)
    //    {
    //        if (m_SetScale)
    //        {
    //            m_CrosshairMaterial.SetFloat("Vector1_707d150ccf4e470db716e1a55b17515d", m_CurrentScale);
    //            m_SetScale = false;
    //            m_UpdateTimer = 0.0f;
    //        }
    //    }
    //    m_UpdateTimer += Time.deltaTime;
    //}
}
