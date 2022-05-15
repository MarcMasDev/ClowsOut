using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Vector3 m_PointColision;
    protected Vector3 m_Pos;
    protected float m_Speed;
    
    protected GameObject m_CollidedObject;
    protected float m_DamageBullet;

    private Vector3 m_NextFramePos;
    protected Vector3 m_Normal;

    protected LayerMask m_CollisionMask;
    protected LayerMask m_CollisionWithEffect;

    public virtual void SetBullet(Vector3 position, Vector3 normal, float speed,
        float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        transform.position = position;
        m_Speed = speed;
        m_CollisionMask = collisionMask;
        m_CollisionWithEffect = collisionWithEffect;
        m_Normal = normal;
        m_DamageBullet = damage;
    }

    public virtual void SetAttractor(float attractorArea, float attractingTime, float attractingDistance,GameObject Particles) {}
    public virtual void SetIce(int maxIterations, float timeIteration, float slowSpeed) { }
    public virtual void SetSticky(float timeExplosion) { }
    public virtual void SetTeleport(GameObject playerMesh, GameObject trailTeleport,float velocityPlayer,GameObject particles) { }
    public virtual void SetEnegy(List<EnergyBullet> eBullets) { }

    public bool Hit()
    {
        RaycastHit l_RayCastHit;
        float l_Time = Time.deltaTime;
        m_NextFramePos = transform.position + m_Normal.normalized * l_Time * m_Speed;

        if (Physics.Raycast(transform.position, m_Normal, out l_RayCastHit, Vector3.Distance(transform.position, m_NextFramePos), m_CollisionMask))
        {
            if (m_CollisionWithEffect == (m_CollisionWithEffect | (1 << l_RayCastHit.collider.gameObject.layer)))
            {
                m_PointColision = l_RayCastHit.point;
                m_CollidedObject = l_RayCastHit.collider.gameObject;
                OnCollisionWithEffect();
            }
            else
            {
                m_PointColision = l_RayCastHit.point;
                m_CollidedObject = l_RayCastHit.collider.gameObject;
                OnCollisionWithoutEffect();
            }
            transform.position = l_RayCastHit.point;

            return true;
        }
        return false;
    }

    public void Move()
    {
        transform.position = m_NextFramePos;
    }

    //TODO: Override effects in each child of bullet
    public virtual void OnCollisionWithEffect()
    {
        Debug.Log("Impact WITH Effect");
    }

    public virtual void OnCollisionWithoutEffect()
    {
        Debug.Log("Impact WITHOUT Effect");
    }
}
