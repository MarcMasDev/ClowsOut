using System.Collections;
using UnityEngine;

public class LinqBullet : Bullet
{
    public bool m_IsHit = false;
    Collider m_Sphere;
    ParticleSystem[] m_FX;

    void Start()
    {
        m_IsHit = false;
        m_Sphere = GetComponent<Collider>();
        m_FX = GetComponentsInChildren<ParticleSystem>();
        
        m_Sphere.enabled = false;
    }
    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }
    public override void OnCollisionWithEffect()
    {
        for (int i = 0; i < m_FX.Length; i++)
        {
            m_FX[i].gameObject.SetActive(true);
            m_FX[i].Play();
            m_FX[i].transform.parent = null;
        }
        m_IsHit = true;
        m_Sphere.enabled = true;
        base.OnCollisionWithEffect();
        
        StartCoroutine(DestroyWithDelay());
    }
    public override void OnCollisionWithoutEffect()
    {
        for (int i = 0; i < m_FX.Length; i++)
        {
            m_FX[i].gameObject.SetActive(true);
            m_FX[i].Play();
            m_FX[i].transform.parent = null;
        }
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
        for (int i = 0; i < m_FX.Length; i++)
        {
            m_FX[i].Stop();
        }
        yield return new WaitForSeconds(0.5f);
        m_Sphere.enabled = false;

        // yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => !m_FX[0].isEmitting);

        Destroy(gameObject);
    }
}
