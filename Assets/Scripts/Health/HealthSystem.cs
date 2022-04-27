using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IRestart
{
    public float m_MaxLife = 100;
    [SerializeField] private float m_CurrentLife;

    public Action<float> OnHit;
    public Action<GameObject> OnDeath;

    private void Start()
    {
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
        print("Damage to" + transform.name + "una cantidad de " + damage);

        float l_CurrDamage = Math.Abs(damage);

        m_CurrentLife -= l_CurrDamage;

        if (m_CurrentLife <= 0)
        {
            OnDeath?.Invoke(gameObject);
            //Die();
        }
        else
        {
            OnHit?.Invoke(m_CurrentLife / m_MaxLife);
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
            m_CurrentLife += l_CurrHealth;
        }
    }
    public virtual void Die()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    public void AddRestartElement()
    {
        RestartElements.m_Instance.addRestartElement(this);
    }

    public void Restart()
    {
        m_CurrentLife = m_MaxLife;
        OnHit?.Invoke(m_CurrentLife / m_MaxLife);
    }
}
