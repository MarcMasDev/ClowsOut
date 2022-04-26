using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBullet : Bullet
{
    private float m_TimeToExplosion;
    private float m_ExplosionArea;

    SphereCollider m_Collider;
    ParticleSystem m_Explosion;
    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void SetSticky(float timeExplosion, float explosionArea)
    {
        m_Explosion = GetComponentInChildren<ParticleSystem>();
         
        m_Collider = GetComponent<SphereCollider>();
        m_Collider.enabled = false;
        m_TimeToExplosion = timeExplosion;
        m_ExplosionArea = explosionArea;
    }

    public override void OnCollisionWithEffect()
    {
        StartCoroutine(DelayExplosion());
    }

    public override void OnCollisionWithoutEffect()
    {
        StartCoroutine(DelayExplosion());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_CollisionWithEffect == (m_CollisionWithEffect | (1 << other.gameObject.layer)))
        {
            other.GetComponent<HealthSystem>().TakeDamage(m_DamageBullet);
        }
        m_Explosion.Play();
    }

    IEnumerator DelayExplosion()
    {
        float l_Time = 0;
        transform.parent = m_CollidedObject.transform;
        while (l_Time <= m_TimeToExplosion)
        {
        
            l_Time += Time.deltaTime;
            yield return null;
        }
        m_Collider.enabled = true;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
