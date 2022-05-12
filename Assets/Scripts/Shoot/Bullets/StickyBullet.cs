using System.Collections;
using UnityEngine;

public class StickyBullet : Bullet
{
    private float m_TimeToExplosion;

    SphereCollider m_Collider;
    ParticleSystem m_Explosion;
    [SerializeField] GameObject bulletSticky;
    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {

        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void SetSticky(float timeExplosion)
    {
        m_Explosion = GetComponentInChildren<ParticleSystem>();

        m_Collider = GetComponent<SphereCollider>();
        m_Collider.enabled = false;
        m_TimeToExplosion = timeExplosion;
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
            if (LinqSystem.m_Instance.ApplyDamageToMarkEnemies(m_DamageBullet, other.gameObject))
            { }
            else
            {
                other.GetComponent<HealthSystem>().TakeDamage(m_DamageBullet);
            }
        }
    }

    IEnumerator DelayExplosion()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.parent = m_CollidedObject.transform;
        yield return new WaitForSeconds(m_TimeToExplosion);
        m_Explosion.transform.parent = null;
        m_Explosion.Play();
        bulletSticky.SetActive(false);
        print(m_Explosion.isEmitting);
        m_Collider.enabled = true;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
