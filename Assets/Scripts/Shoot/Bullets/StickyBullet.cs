using System.Collections;
using UnityEngine;

public class StickyBullet : Bullet
{
    [SerializeField]
    private float m_TimeToExplosion;

    SphereCollider m_Collider;
    [SerializeField] GameObject bulletSticky;
    [SerializeField] PlayParticle explosionFX;
    [SerializeField]
    bool m_DroneBullet = false;
    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect, Transform enemy_transform = null)
    {

        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect, enemy_transform);
    }

    public override void SetSticky(float timeExplosion)
    {
        m_Collider = GetComponent<SphereCollider>();
        m_Collider.enabled = false;
        if (!m_DroneBullet)
        {
            m_TimeToExplosion = timeExplosion;
        }
        
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
            print("daño a player");
            if (LinqSystem.m_Instance.ApplyDamageToMarkEnemies(m_DamageBullet, other.gameObject))
            { }
            else
            {
                other.GetComponent<HealthSystem>().TakeDamage(m_DamageBullet);
                print("daño a player2");
            }
        }
    }

    IEnumerator DelayExplosion()
    {
        print("daño sticky IEnum");
        transform.GetChild(0).gameObject.SetActive(false);
        transform.parent = m_CollidedObject.transform;
        yield return new WaitForSeconds(m_TimeToExplosion);
        explosionFX.transform.parent = null;
        explosionFX.PlayParticles();
        bulletSticky.SetActive(false);
        m_Collider.enabled = true;
        print("daño sticky IEnum trigger activado");
        yield return new WaitForSeconds(0.2f);
        m_Collider.enabled = false;
        print("daño sticky IEnum trigger desactivado");
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
