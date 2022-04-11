using UnityEngine;

public class Bullet
{
    public float m_Speed;
    private Vector3 m_Direction;
    private Vector3 m_Pos;
    private Vector3 m_NextFramePos;

    public bool HitSomething;

    public Bullet(Vector3 position, Vector3 directions, float speed)
    {
        m_Pos = position;
        m_Direction = directions;
        m_Speed = speed;
    }

    public bool Hit()
    {
        RaycastHit l_RayCastHit;
        float l_Time = Time.deltaTime;
        m_NextFramePos = m_Pos + m_Direction.normalized * l_Time;

        if (Physics.Raycast(m_Pos, m_Direction, out l_RayCastHit, Vector3.Distance(m_Pos, m_NextFramePos)))
        {

            if (l_RayCastHit.collider.CompareTag("Enemy"))
            {
                HitSomething = true;
                //l_RayCastHit.collider.GetComponent<>().Hit();
            }
            else if (l_RayCastHit.collider.CompareTag("Collisionable"))
            {
                HitSomething = true;
            }
            else
            {
                HitSomething = false;
            }

            m_Pos = l_RayCastHit.point;
            return true;

        }
        return false;
    }

    public void Move()
    {
        m_Pos = m_NextFramePos;
    }
}
