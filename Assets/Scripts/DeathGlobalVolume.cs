using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DeathGlobalVolume : MonoBehaviour
{
    private bool m_Death;
    private float m_Timer;
    [SerializeField] private float m_Time;
    [SerializeField] private Volume m_Volume;
    private void Start()
    {
        m_Volume.weight = 0;
    }
    private void OnEnable()
    {
        Player_Death.m_OnDeathS += OnDeath;
        Player_Death.m_OnReviveS += OnRevive;
    }
    private void OnDisable()
    {
        Player_Death.m_OnDeathS -= OnDeath;
        Player_Death.m_OnReviveS -= OnRevive;
    }
    private void Update()
    {
        if (m_Death)
        {
            if(m_Time >= m_Timer)
            {
                m_Death = false;
                m_Timer = 0;
            }
            Mathf.Lerp(0, 1, m_Timer / m_Time);
            m_Timer += Time.deltaTime;
        }
    }
    private void OnDeath()
    {
        m_Death = true;
        m_Timer = 0;
        m_Volume.weight = 1;
    }
    private void OnRevive()
    {
        m_Volume.weight = 0;
    }
}
