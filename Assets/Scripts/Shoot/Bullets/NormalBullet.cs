using UnityEngine;

public class NormalBullet : Bullet
{
    HealthSystem m_HealthSystem;

    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void OnCollisionWithEffect()
    {

        m_HealthSystem = m_CollidedObject.GetComponent<HealthSystem>();
        if (m_HealthSystem != null)
        {
            m_HealthSystem.TakeDamage(m_DamageBullet);
            if (!m_CollidedObject.CompareTag("Player"))
            {
                LinqSystem.m_Instance.AplyDamageToMarkEnemies(m_DamageBullet, m_CollidedObject);
            }
        }
           

        Destroy(gameObject);
        //Debug.Log("Restado da�o a "+m_HealthSystem);
    }

    public override void OnCollisionWithoutEffect()
    {
        //Debug.Log("Colision sin quitar da�o");
        Destroy(gameObject);
    }
}
