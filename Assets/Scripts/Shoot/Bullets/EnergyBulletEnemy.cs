using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBulletEnemy : Bullet
{
    HealthSystem m_HealthSystem;
    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void SetEnegy(List<EnergyBullet> eBullets)
    {
        //m_CurrEnergyBullet = eBullets;
    }
    public override void OnCollisionWithEffect()
    {
        m_HealthSystem = m_CollidedObject.GetComponent<HealthSystem>();
        if (LinqSystem.m_Instance.ApplyDamageToMarkEnemies(m_DamageBullet, m_CollidedObject)) { }
        else
        {
            m_HealthSystem.TakeDamage(m_DamageBullet);
        }

        Destroy(gameObject);
    }
    public override void OnCollisionWithoutEffect() { Destroy(gameObject); }

}
