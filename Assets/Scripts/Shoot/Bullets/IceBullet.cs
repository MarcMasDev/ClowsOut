using System.Collections;
using UnityEngine;

public class IceBullet : Bullet
{
    HealthSystem m_EnemyHealthSystem;
    
    IEnumerator routine;
    int m_MaxIterations;
    float m_TimeBetweenIteration;
    float m_PreviousSpeed;

    
    public IceBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect, int maxIterations, float timeIteration) : base(position, normal, speed, damage, collisionMask, collisionWithEffect)
    {
        m_MaxIterations = maxIterations;
        m_TimeBetweenIteration = timeIteration;
    }

    public override void OnCollisionWithEffect()
    {
        m_EnemyHealthSystem = m_CollidedObject.GetComponent<HealthSystem>();
        routine = TemporalDamage();
        ControlCoroutines l_Control = GameObject.FindObjectOfType<ControlCoroutines>();
        l_Control.StartingCoroutine(routine);
    }

    public override void OnCollisionWithoutEffect()
    {
        Debug.Log("Colision WITHOUT ice effect");
    }

    IEnumerator TemporalDamage()
    {
        int l_CurrIterations =0;
        while (l_CurrIterations < m_MaxIterations)
        {
            yield return new WaitForSeconds(m_TimeBetweenIteration);
            m_EnemyHealthSystem.TakeDamage(m_DamageBullet);
            l_CurrIterations++;
        }

        Debug.Log("Temporal Damage Finished");
    }
}
