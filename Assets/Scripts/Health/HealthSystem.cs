using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IRestart
{
    public float m_MaxLife = 100;
    [SerializeField] private float m_CurrentLife;

    public Action<float> m_OnHit;
    public Action<GameObject> m_OnDeath;

    private bool m_Dead;

    public bool m_Debugging;
    private void Start()
    {
        if (m_Debugging)
            m_CurrentLife = 10000;
        else
        m_CurrentLife = m_MaxLife;
        AddRestartElement();
    }

    /// <summary>
    /// Taking health and checking if it could die. Absolute values
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage)
    {
        //to avoid negative values because we don't want to heal in this method.

        float l_CurrDamage = Math.Abs(damage);

        m_CurrentLife -= l_CurrDamage;

        if (m_CurrentLife <= 0 && !m_Dead)
        {
            m_OnDeath?.Invoke(gameObject);
            m_Dead = true;
        }
        else
        {
            m_OnHit?.Invoke(m_CurrentLife / m_MaxLife);
        }
    }

    /// <summary>
    /// Adding health. Absolute values
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeHealth(float health)
    {
        float l_CurrHealth = m_CurrentLife + Math.Abs(health);

        if (l_CurrHealth > m_MaxLife)
        {
            m_CurrentLife = m_MaxLife;
        }
        else
        {
            m_CurrentLife = l_CurrHealth;
            m_OnHit?.Invoke(m_CurrentLife / m_MaxLife);
        }
    }
    public virtual void Die()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    public void AddRestartElement()
    {
        GameManager.GetManager().GetRestartManager().addRestartElement(this);
    }
    public void UpdateHealthBar()
    {
        m_OnHit?.Invoke(m_CurrentLife / m_MaxLife);
    }

    public void Restart()
    {
        m_CurrentLife = m_MaxLife;
        m_OnHit?.Invoke(m_CurrentLife / m_MaxLife);
        m_Dead = false;
    }
}
