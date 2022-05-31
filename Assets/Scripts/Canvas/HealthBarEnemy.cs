using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour
{
    [SerializeField]
    Image m_HealthBar;

     public HealthSystem m_hp;
    private void Start()
    {
        if (m_hp != null)
        {
            m_hp.m_OnHit += SetValue;
            m_hp.m_OnDeath += OnDeath;
        }
            
    }
    private void OnEnable()
    {
        if (m_hp != null)
        {
            m_hp.m_OnHit += SetValue;
            m_hp.m_OnDeath += OnDeath;
        }
           
    }
    private void OnDisable()
    {
        m_hp.m_OnHit -= SetValue;
    }

    public void SetValue(float amount)
    {
        m_HealthBar.fillAmount = amount;
    }
    public void OnDeath(GameObject a)
    {//TODO pooling 
        gameObject.SetActive(false);
       // Destroy(gameObject);
    }
}
