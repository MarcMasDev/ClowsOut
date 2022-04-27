using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinqBullet : Bullet
{
    public bool m_IsHit = false;
    Collider m_Sphere;
    ParticleSystem fx;
    // Start is called before the first frame update
    void Start()
    {
        m_IsHit = false;
        m_Sphere = GetComponent<Collider>();
        fx = GetComponentInChildren<ParticleSystem>();
        fx.gameObject.SetActive(false);
        m_Sphere.enabled = false;
    }
    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }
    public override void OnCollisionWithEffect()
    {
        base.OnCollisionWithEffect();
        m_IsHit = true;
        m_Sphere.enabled = true;
        StartCoroutine(DestroyWithDelay());
    }
    public override void OnCollisionWithoutEffect()
    {
        m_IsHit = true;
        m_Sphere.enabled = true;
        fx.gameObject.SetActive(true);

        base.OnCollisionWithoutEffect();

        StartCoroutine(DestroyWithDelay());
    }

    private void OnTriggerEnter(Collider other)
    {
        // para no hacer get component en OnTrigger  podemos poner este trigger ene enemy
        if (m_IsHit && m_CollisionWithEffect == (m_CollisionWithEffect | (1 << other.gameObject.layer)))
        {
            BlackboardEnemies l_Blackboard = other.GetComponent<BlackboardEnemies>();
            l_Blackboard.SetIsLinq();
            LinqSystem.m_Instance.AddLinqued(l_Blackboard);
        }
    }
    IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
