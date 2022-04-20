using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour
{
     Image m_HealthBar;

     public HealthSystem m_hp;
    private void Start()
    {
        m_HealthBar = GetComponent<Image>();
        if (m_hp != null)
            m_hp.OnHit += SetValue;
    }
    private void OnEnable()
    {
        if (m_hp != null)
            m_hp.OnHit += SetValue;
    }
    private void OnDisable()
    {
        m_hp.OnHit -= SetValue;
    }

    public void SetValue(float amount)
    {
        m_HealthBar.fillAmount = amount;
    }
}
