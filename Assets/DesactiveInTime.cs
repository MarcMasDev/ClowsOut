using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactiveInTime : MonoBehaviour
{
    float m_timer = 0f;
    [SerializeField]
    float m_TimeToDesactive=2f;
    [SerializeField]
    float m_TimeToDesactiveCollider = 0.2f;
    ParticleSystem m_ParticleSystem;
    Collider m_Collider;
    [SerializeField]
    LayerMask m_layerCollide;
    [SerializeField]
    float m_damage = 10f;
    private void Awake()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Collider = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        m_ParticleSystem.Play();
        m_Collider.enabled = true;
    }
    private void Update()
    {
        m_timer += Time.deltaTime;
        if(m_timer > m_TimeToDesactive)
        {
            m_timer = 0f;
            gameObject.SetActive(false);
        }
        if(m_timer > m_TimeToDesactiveCollider)
        {
            m_Collider.enabled = false;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (m_layerCollide == (m_layerCollide | (1 << other.gameObject.layer)))
        {
            
            if (LinqSystem.m_Instance.ApplyDamageToMarkEnemies(m_damage, other.gameObject)) { }
            else { other.GetComponent<HealthSystem>().TakeDamage(m_damage); }
        }
    }
}
