using System.Collections;
using UnityEngine;

public class IceBullet : Bullet
{
    HealthSystem m_EnemyHealthSystem;
    FSM_AI m_Enemy;
    IEnumerator routine;
    int m_MaxIterations;
    float m_TimeBetweenIteration;
    float m_PreviousSpeed=10;

    float m_SlowSpeed;
    
    public IceBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect,
        int maxIterations, float timeIteration, float slowSpeed) : base(position, normal, speed, damage, collisionMask, collisionWithEffect)
    {
        m_MaxIterations = maxIterations;
        m_TimeBetweenIteration = timeIteration;
        m_SlowSpeed = slowSpeed;
    }

    public override void OnCollisionWithEffect()
    {
        m_Enemy = m_CollidedObject.GetComponent<FSM_AI>();
        m_EnemyHealthSystem = m_CollidedObject.GetComponent<HealthSystem>();

        routine = TemporalDamage();
        ControlCoroutines l_Control = GameObject.FindObjectOfType<ControlCoroutines>();
        l_Control.StartingCoroutine(routine);

        m_Enemy.ChangeSpeed(m_SlowSpeed);
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

        m_Enemy.ChangeSpeed(m_PreviousSpeed);
        Debug.Log("Temporal Damage Finished");
    }
}
