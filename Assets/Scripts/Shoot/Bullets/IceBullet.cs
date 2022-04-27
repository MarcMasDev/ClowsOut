using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IceBullet : Bullet
{
    HealthSystem m_EnemyHealthSystem;

    int m_MaxIterations;
    float m_TimeBetweenIteration;
    float m_PreviousSpeed = 7;

    float m_SlowSpeed = 3.5f;
    NavMeshAgent m_Enemy;

    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void SetIce(int maxIterations, float timeIteration, float slowSpeed)
    {
        Debug.Log("Set Ice Bullet");
        m_MaxIterations = maxIterations;
        m_TimeBetweenIteration = timeIteration;
        m_SlowSpeed = slowSpeed;
    }

    public override void OnCollisionWithEffect()
    {
        m_EnemyHealthSystem = m_CollidedObject.GetComponent<HealthSystem>();
        m_Enemy = m_CollidedObject.GetComponent<NavMeshAgent>();
        m_PreviousSpeed = m_Enemy.speed;
        m_Enemy.speed = m_SlowSpeed;
        
        if (!m_CollidedObject.CompareTag("Player"))
        {
            if (LinqSystem.m_Instance.IceBullet(
                    m_MaxIterations,
                    m_DamageBullet,
                    m_TimeBetweenIteration,
                    m_SlowSpeed))
            {
                Destroy(gameObject);
            }
            else
            {
                m_CollidedObject.GetComponent<IceState>().StartStateIce();
                StartCoroutine(TemporalDamage());
            }
        }
       
       
    }
    public override void OnCollisionWithoutEffect()
    {
        Debug.Log("Colision WITHOUT ice effect");
    }

    IEnumerator TemporalDamage()
    {
        int l_CurrIterations = 0;
        HealthSystem l_EnemyHealthSystem = m_EnemyHealthSystem;//Por si cambia la miembro
        while (l_CurrIterations < m_MaxIterations)
        {
            l_EnemyHealthSystem.TakeDamage(m_DamageBullet);
            yield return new WaitForSeconds(m_TimeBetweenIteration);
            l_CurrIterations++;
        }
        m_Enemy.speed = m_PreviousSpeed;
        Destroy(gameObject);
    }
}
