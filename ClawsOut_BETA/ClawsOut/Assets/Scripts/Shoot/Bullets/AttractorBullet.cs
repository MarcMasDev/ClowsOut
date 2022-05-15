using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttractorBullet : Bullet
{
    private float m_AttractorArea;
    private float m_AttractingTime;
    private float m_RequireAttractorDistance;
    List<GameObject> m_Enemies = new List<GameObject>();
    List<Vector3> m_InitialPos = new List<Vector3>();
    List<Vector3> m_FinalPos = new List<Vector3>();
    private SphereCollider m_Collider;
    GameObject m_ParticleAttactor;
    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void SetAttractor(float attractorArea, float attractingTime, float attractingDistance,GameObject Particles)
    {
        m_Collider = GetComponent<SphereCollider>();
        m_Collider.enabled = false;
        m_AttractorArea = attractorArea;
        m_AttractingTime = attractingTime;
        m_RequireAttractorDistance = attractingDistance;
        m_ParticleAttactor = Particles;
    }

    public override void OnCollisionWithEffect()
    {
        m_Collider.enabled = true;
        GameObject l_gb = GameObject.Instantiate(m_ParticleAttactor,transform.position, Quaternion.identity);
        print("intancia ");
        StartCoroutine(DestroyWithDelay());
    }

    public override void OnCollisionWithoutEffect()
    {
        m_Collider.enabled = true;
        GameObject l_gb = GameObject.Instantiate(m_ParticleAttactor, transform.position, Quaternion.identity);

        StartCoroutine(DestroyWithDelay());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & m_CollisionWithEffect) != 0)
        {
            m_Enemies.Add(other.gameObject);
            other.gameObject.GetComponent<BlackboardEnemies>().ActivateAttractorEffect(
            m_PointColision);
        }
        /*if (m_CollisionWithEffect == (m_CollisionWithEffect | (1 << other.gameObject.layer)))
        {
            m_Enemies.Add(other.gameObject);
            print(other.gameObject.name);
            other.gameObject.GetComponent<BlackboardEnemies>().ActivateAttractorEffect(
            m_PointColision);
        }*/
    }
    IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
