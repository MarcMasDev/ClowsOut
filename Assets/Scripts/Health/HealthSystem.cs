using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float m_MaxLife=100;
    [SerializeField]private float m_CurrentLife;

    private void Start()
    {
        m_CurrentLife = m_MaxLife;
    }

    /// <summary>
    /// Taking health and checking if it could die.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage)
    {
        float l_CurrDamage=damage;

        //to avoid negative values because we don't want to heal in this method. 
        if (l_CurrDamage < 0)
            l_CurrDamage = -damage;

        if (l_CurrDamage >= m_MaxLife)
        {
            Die();
        }
        else
        {
            m_CurrentLife -= l_CurrDamage;
        }
    }

    /// <summary>
    /// Adding health.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeHealth(float health)
    {
        float l_CurrHealth = m_CurrentLife + health;

        if (l_CurrHealth > m_MaxLife)
        {
            m_CurrentLife = m_MaxLife;
        }
        else
        {
            m_CurrentLife += health;
        }
    }
    public virtual void Die()
    { }
}
