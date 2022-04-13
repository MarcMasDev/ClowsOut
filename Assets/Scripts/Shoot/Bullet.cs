using UnityEngine;

public class Bullet
{
    public float m_Speed;
    private Vector3 m_Pos;
    private Vector3 m_NextFramePos;
    private Vector3 m_Normal;

    private LayerMask m_CollisionMask;
    private LayerMask m_CollisionWithEffect;

    public Bullet(Vector3 position, Vector3 normal, float speed, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        m_Pos = position;
        m_Speed = speed;
        m_CollisionMask = collisionMask;
        m_CollisionWithEffect = collisionWithEffect;
        m_Normal = normal;
    }

    public bool Hit()
    {
        RaycastHit l_RayCastHit;
        float l_Time = Time.deltaTime;
        m_NextFramePos = m_Pos + m_Normal.normalized * l_Time;
        Debug.DrawLine(m_Pos, m_NextFramePos);
        if (Physics.Raycast(m_Pos, m_Normal, out l_RayCastHit, Vector3.Distance(m_Pos, m_NextFramePos), m_CollisionMask))
        {
            Debug.Log("shot");
            if (m_CollisionMask == (m_CollisionMask | (1 << m_CollisionWithEffect)))
            {
                PlayEffect();
            }
            else
                PlayWithoutEffect();

            m_Pos = l_RayCastHit.point;
            return true;

        }
        return false;
    }

    public void Move()
    {
        m_Pos = m_NextFramePos;
    }

    public virtual void PlayEffect()
    {

        {
            Debug.Log("shot");
        }
    }

    public virtual void PlayWithoutEffect()
    { 
    
    
    }

}
