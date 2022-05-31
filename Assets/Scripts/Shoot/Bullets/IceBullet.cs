using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IceBullet : Bullet
{
    [SerializeField] List<HealthSystem> m_EnemyHealthSystem = new List<HealthSystem>();
    [SerializeField] List<BlackboardEnemies> m_EnemyControl = new List<BlackboardEnemies>();
    int m_MaxIterations;
    float m_TimeBetweenIteration;
    float m_PreviousSpeed = 7;

    float m_SlowSpeed = 3.5f;
    List<NavMeshAgent> m_Enemy = new List<NavMeshAgent>();
    SphereCollider m_Collider;
    float m_Counter;

    [SerializeField] private PlayParticle iceExplosion;
    private void Awake()
    {
        m_Collider = GetComponent<SphereCollider>();
    }
    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect, Transform enemy_transform = null)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect, enemy_transform);
    }

    public override void SetIce(int maxIterations, float timeIteration, float slowSpeed)
    {
        //Debug.Log("Set Ice Bullet");
        m_MaxIterations = maxIterations;
        m_TimeBetweenIteration = timeIteration;
        m_SlowSpeed = slowSpeed;
    }

    private void Update()
    {
        base.Update();

        if (m_Collider.enabled)
            m_Counter += Time.deltaTime;

        if (m_Counter > 0.25f && m_Collider.enabled)
        {
            m_Collider.enabled = false;
            EffectIce();
            iceExplosion.PlayParticles();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        BlackboardEnemies l_Enemy = other.GetComponent<BlackboardEnemies>();
        if (l_Enemy!=null&& !m_EnemyControl.Contains(l_Enemy) && !l_Enemy.CompareTag("Player"))
        {
            m_EnemyControl.Add(l_Enemy);
            Debug.Log("ice " + other.gameObject.name);
        }
    }

    private void EffectIce()
    {
        for (int i = 0; i < m_EnemyControl.Count; i++)
        {
            m_EnemyHealthSystem.Add(m_EnemyControl[i].m_hp);
            m_Enemy.Add(m_EnemyControl[i].m_nav);
            m_PreviousSpeed = m_Enemy[i].speed;
            m_Enemy[i].speed = m_SlowSpeed;

            if (LinqSystem.m_Instance.IceBullet(
                    m_MaxIterations,
                    m_DamageBullet,
                    m_TimeBetweenIteration,
                    m_SlowSpeed,
                    m_EnemyControl[i].gameObject))
            {
                Destroy(gameObject);
            }
            else
            {
                m_EnemyControl[i].isIceState = true;
                m_EnemyControl[i].GetComponent<IceState>().StartStateIce();
                StartCoroutine(TemporalDamage(i));
            }
        }
    }

    public override void OnCollisionWithEffect()
    {
        m_Collider.enabled = true;
    }
    public override void OnCollisionWithoutEffect()
    {
        m_Collider.enabled = true;
    }

    IEnumerator TemporalDamage(int index)
    {
        int l_CurrIterations = 0;
        HealthSystem l_EnemyHealthSystem = m_EnemyHealthSystem[index];
        while (l_CurrIterations < m_MaxIterations)
        {
            l_EnemyHealthSystem.TakeDamage(m_DamageBullet);
            yield return new WaitForSeconds(m_TimeBetweenIteration);
            l_CurrIterations++;
        }
        m_Enemy[index].speed = m_PreviousSpeed;
        m_EnemyControl[index].isIceState = false;

        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
