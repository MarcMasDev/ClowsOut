using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDamage : MonoBehaviour
{
    public float m_DamageBullet=0f;
    float m_timer =0f;
    Collider m_collider;
    private void Awake()
    {
        m_collider = GetComponent<Collider>();
    }
    private void Update()
    {
        if (m_collider.enabled)
        {
            m_timer += Time.deltaTime;
            if (m_timer > 0.2f)
            {
                m_collider.enabled = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("TP damage " + m_DamageBullet);
            if (LinqSystem.m_Instance.ApplyDamageToMarkEnemies(m_DamageBullet, other.gameObject))
            { }
            else
            {
                other.GetComponent<HealthSystem>().TakeDamage(m_DamageBullet);
            }
        }

    }
}
