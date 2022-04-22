using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class IceBullet : Bullet
{
    HealthSystem m_EnemyHealthSystem;
    IEnumerator m_Routine;

    int m_MaxIterations;
    float m_TimeBetweenIteration;
    float m_PreviousSpeed = 7;

    float m_SlowSpeed = 3.5f;
    NavMeshAgent m_Enemy;

    ControlCoroutines m_Control;

    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        Debug.Log("Set Bullet");
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
        // m_Enemy = m_CollidedObject.GetComponent<FSM_AI>();
        // m_Enemy.ChangeSpeed(m_SlowSpeed);

        m_EnemyHealthSystem = m_CollidedObject.GetComponent<HealthSystem>();
        m_Enemy = m_CollidedObject.GetComponent<NavMeshAgent>();
        m_PreviousSpeed = m_Enemy.speed;
        m_Enemy.speed = m_SlowSpeed;

        Debug.Log(m_Enemy);
        m_Routine = TemporalDamage();
        m_Control = GameObject.FindObjectOfType<ControlCoroutines>();
        m_Control.StartingCoroutine(m_Routine);
    }

    public override void OnCollisionWithoutEffect()
    {
        Debug.Log("Colision WITHOUT ice effect");
    }

    IEnumerator TemporalDamage()
    {
        int l_CurrIterations = 0;
        while (l_CurrIterations < m_MaxIterations)
        {
            yield return new WaitForSeconds(m_TimeBetweenIteration);
            m_EnemyHealthSystem.TakeDamage(m_DamageBullet);
            l_CurrIterations++;
        }

        m_Enemy.speed = m_PreviousSpeed;
        // m_Enemy.ChangeSpeed(m_PreviousSpeed);
        Debug.Log("Temporal Damage Finished");
        m_Control.StopingCoroutine(m_Routine);
    }
}
