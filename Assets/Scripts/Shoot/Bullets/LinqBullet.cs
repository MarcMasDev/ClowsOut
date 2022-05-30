using System.Collections;
using UnityEngine;

public class LinqBullet : Bullet
{
    public bool m_IsHit = false;
    Collider m_Sphere;
    [SerializeField] private Animator fx;
    void Start()
    {
        m_IsHit = false;
        m_Sphere = GetComponent<Collider>();

        m_Sphere.enabled = false;
    }
    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect, Transform enemy_transform = null)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect, enemy_transform);
    }
    public override void OnCollisionWithEffect()
    {
        m_IsHit = true;
        m_Sphere.enabled = true;

        base.OnCollisionWithEffect();
        
        StartCoroutine(DestroyWithDelay());
    }
    public override void OnCollisionWithoutEffect()
    {
        m_IsHit = true;
        m_Sphere.enabled = true;

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
        }
    }
    IEnumerator DestroyWithDelay()
    {
        fx.gameObject.SetActive(true);
        fx.SetBool("Link", true);
        fx.transform.SetParent(null);
        yield return new WaitForSeconds(0.5f);
        m_Sphere.enabled = false;
        Destroy(gameObject);
    }
}
