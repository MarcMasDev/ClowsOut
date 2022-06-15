using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IRestart
{
    public float m_MaxLife = 100;
    [SerializeField] private float m_CurrentLife;
    [SerializeField] private int healthAmount = 25;

    public float GetCurrentLife
    {
        get { return m_CurrentLife; }
    }
    public Action<float> m_OnHit;
    public Action<GameObject> m_OnDeath;
    public bool m_reciveDamage = false;
    public bool m_Dead;

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
        m_reciveDamage = true;
        float l_CurrDamage = Math.Abs(damage);
        m_CurrentLife -= l_CurrDamage;

        //print(m_CurrentLife);

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
    public virtual void TakeHealth()
    {
        float l_CurrHealth = m_CurrentLife + healthAmount;

        if (l_CurrHealth > m_MaxLife)
        {
            m_CurrentLife = m_MaxLife;
        }
        else
        {
            m_CurrentLife = l_CurrHealth;
        }
        m_OnHit?.Invoke(m_CurrentLife / m_MaxLife);
    }
    public virtual void Die()
    {
        //gameObject.SetActive(false);
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
        m_Dead = false;
    }
}
