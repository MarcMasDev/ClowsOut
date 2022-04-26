using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class EnergyBullet : Bullet
{
    HealthSystem m_HealthSystem;
    GameObject m_Enemy;

    List<EnergyBullet> m_CurrEnergyBullet;
    public List<int> m_EnemiesDetected;

    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void SetEnegy(List<EnergyBullet> eBullets)
    {
        m_CurrEnergyBullet = eBullets;
    }
    public override void OnCollisionWithEffect()
    {
        m_HealthSystem = m_CollidedObject.GetComponent<HealthSystem>();
        m_HealthSystem.TakeDamage(m_DamageBullet);

        Destroy(gameObject);
    }
    public override void OnCollisionWithoutEffect() { Destroy(gameObject); }

    private void OnTriggerEnter(Collider other)
    {
        if (m_CollisionWithEffect == (m_CollisionWithEffect | (1 << other.gameObject.layer)))
        {
            HighFSM l_Enemy = other.GetComponent<HighFSM>();

            if (l_Enemy != null && !m_EnemiesDetected.Contains(l_Enemy.m_ID))
            {
                foreach (EnergyBullet item in m_CurrEnergyBullet)
                {
                    item.m_EnemiesDetected.Add(l_Enemy.m_ID);
                }
                m_Enemy = l_Enemy.gameObject;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (m_Enemy != null)
            m_Normal = (m_Enemy.transform.position - transform.position).normalized;
    }
}