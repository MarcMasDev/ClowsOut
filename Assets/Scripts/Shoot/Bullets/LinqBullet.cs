using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class LinqBullet : Bullet
{
    bool m_IsHit = false;
    Collider m_Sphere;
    // Start is called before the first frame update
    void Start()
    {
        m_IsHit = false;
        m_Sphere = GetComponent<Collider>();
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
    }

    private void OnTriggerEnter(Collider other)
    {
        // para no hacer get component en OnTrigger  podemos poner este trigger ene enemy
        if (m_IsHit && m_CollisionWithEffect == (m_CollisionWithEffect | (1 << other.gameObject.layer)))
        {
            other.GetComponent<BlackboardEnemies>().SetIsLinq();
            LinqSystem.m_Instance.AddLinqued(other.gameObject);
        }
    }
}
