using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class LinqBullet : Bullet
{
    bool m_IsHit = false;
    // Start is called before the first frame update
    void Start()
    {
        m_IsHit = false;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }
    public override void OnCollisionWithEffect()
    {
        base.OnCollisionWithEffect();
        m_IsHit = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_IsHit && m_CollisionWithEffect == (m_CollisionWithEffect | (1 << other.gameObject.layer)))
        {
            other.GetComponent<BlackboardEnemies>().m_IsLinq = true;
        }
    }
}
