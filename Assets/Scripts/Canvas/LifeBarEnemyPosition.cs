using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarEnemyPosition : MonoBehaviour
{
    [SerializeField]
    RectTransform m_LifeBar;
    Camera m_Camera;
    InputManager m_input;
    bool m_Aiming = false;
    [SerializeField]
    GameObject m_InconLinqEnemy;
    bool m_InconLinqEnemyShowed = false;
    Vector3 m_offset;
    float m_TimeShowingAfterDamage = 1f;
    bool m_damage = false;
    float m_Timer = 0f;
    private void Start()
    {
        m_Camera = Camera.main;
        m_input = GameObject.FindGameObjectWithTag("Input").GetComponent<InputManager>();
        m_input.OnStartAiming += StartAim;
        m_input.OnStopAiming += StopAim;
        m_offset = m_InconLinqEnemy.transform.position - m_LifeBar.transform.position;
    }
    public void SetLifeBarEnemy(Vector3 WorldPosition, bool isLinq)
    {
        if (m_damage)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer > m_TimeShowingAfterDamage)
            {
                m_damage = false;
            }
        }
        Vector3 l_ViewportPoint = m_Camera.WorldToScreenPoint(WorldPosition);
        m_LifeBar.transform.position = l_ViewportPoint;
        m_InconLinqEnemy.transform.position = l_ViewportPoint + m_offset;
        
        if (l_ViewportPoint.z > 0.0f && (m_Aiming || m_damage))
        {
            m_LifeBar.gameObject.SetActive(true);
        }
        else
        {
            m_LifeBar.gameObject.SetActive(false);
        }
        if (l_ViewportPoint.z > 0.0f)
        {
            if(isLinq)
                ShowLinqIcon();
        }
        else
        {
            HideLinqIcon();
        }
    }
    public void DontShow()//Esta funcion se llama siempre que el player no pueda ver al dron
    {
        m_LifeBar.gameObject.SetActive(false);
        m_InconLinqEnemy.gameObject.SetActive(false);
    }
    public void StartAim()
    {
        m_Aiming = true;
    }
    public void StopAim()
    {
        m_Aiming = false;
    }
    public void ShowLinqIcon()
    {
        if (!m_InconLinqEnemyShowed)
        {
            m_InconLinqEnemyShowed = true;
            m_InconLinqEnemy.SetActive(true);
        }
    }
    public void HideLinqIcon()
    {
        if (m_InconLinqEnemyShowed)
        {
            m_InconLinqEnemyShowed = false;
            m_InconLinqEnemy.SetActive(false);

        }
    }
    public void OnTakeDamage()
    {
        m_Timer = 0f;
        m_damage = true;
    }
}
