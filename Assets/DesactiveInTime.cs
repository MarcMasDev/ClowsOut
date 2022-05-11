using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactiveInTime : MonoBehaviour
{
    float m_timer = 0f;
    public float m_TimeToDesactive=2f;
    ParticleSystem m_ParticleSystem;
    private void Awake()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        m_ParticleSystem.Play();
    }
    private void Update()
    {
        m_timer += Time.deltaTime;
        if(m_timer > m_TimeToDesactive)
        {
            m_timer = 0f;
            gameObject.SetActive(false);
        }

    }
}
