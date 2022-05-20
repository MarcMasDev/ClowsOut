using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowNavMeshAgentFlying : MonoBehaviour
{
    [SerializeField]
    float m_FollowSpeed = 20f; 
    [SerializeField]
    float m_DistanceToCheck = 1f;
    BlackboardEnemies m_blackboardEnemies;
    Vector3[] m_directions = new Vector3[]{
        Vector3.right,
        Vector3.right+Vector3.forward,
        Vector3.forward,
        Vector3.back,
        Vector3.left+Vector3.forward,
        Vector3.left,
        Vector3.up,
        Vector3.down
    };
    [SerializeField]
    Transform[] m_PointsForCHeckDistanceToFloor;
    RaycastHit[] m_hits;
    States m_state;
    Vector3 m_dir = Vector3.zero;
    float m_distanceToFloor, m_distanceToGround;
    [SerializeField]
    float m_MinDistanceToCollision = 5f;
    // Start is called before the first frame update
    void Start()
    {
        m_blackboardEnemies = GetComponent<BlackboardEnemies>();
        m_state = States.FollowNav;
        m_blackboardEnemies.m_Rigibody.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case States.FollowNav:
                CalculateDir();
                CheckCollisions();
                Move();
                break;
            case States.Attractor:
                break;
            case States.ExitAttractor:
                m_blackboardEnemies.m_nav.enabled = true;
                RaycastHit l_hit;
                Physics.Raycast(transform.position, Vector3.down, out l_hit, Mathf.Infinity, m_blackboardEnemies.m_CollisionLayerMask);
                if (l_hit.collider != null)
                {
                    m_blackboardEnemies.m_nav.nextPosition = l_hit.point;
                }
                m_state = States.FollowNav;
                break;
        }
        if (m_blackboardEnemies.m_Pause)
        {
            m_state = States.Attractor;
        }
        else
        {
            if (m_state == States.Attractor)
            {
                m_state = States.ExitAttractor;
            }
        }
    }
    public void CheckCollisions()
    {
        m_hits = new RaycastHit[m_directions.Length];
        for (int i = 0; i < m_directions.Length; i++)
        {
            //Ponemos la direcion en global
            Vector3 dir = transform.TransformDirection(m_directions[i]);
            Physics.Raycast(transform.position, dir, out m_hits[i], m_DistanceToCheck, m_blackboardEnemies.m_CollisionLayerMask);
            if (m_hits[i].collider != null)
            {
                Debug.DrawRay(transform.position, dir * m_hits[i].distance, Color.green);
                m_dir = m_dir +(m_directions[i] * -1);
                m_dir.Normalize();
            }
            else
            {
                Debug.DrawRay(transform.position, dir, Color.red);
            }
        }
    }
    public void CalculateDir()
    {
        m_dir = m_blackboardEnemies.m_nav.transform.position;//Guardamos el punto al que queremos ir para modificarlo antes de calcular la dir
        m_dir.y = 0;//Eliminanos la y para luego recalcular a que altura queremos estar
        RaycastHit l_hit;
        /*Physics.Raycast(transform.position, Vector3.up, out l_hit, Mathf.Infinity, m_blackboardEnemies.m_CollisionLayerMask);
        if (l_hit.collider != null)
        {
            Debug.DrawRay(transform.position, Vector3.up * l_hit.distance, Color.green);
            m_distanceToFloor = l_hit.distance;
        } 
        Physics.Raycast(transform.position, Vector3.down, out l_hit, Mathf.Infinity, m_blackboardEnemies.m_CollisionLayerMask);
        if (l_hit.collider != null)
        {
            Debug.DrawRay(transform.position, Vector3.down * l_hit.distance, Color.green);
            m_distanceToGround = l_hit.distance;
        }
        if(m_distanceToFloor > m_distanceToGround)
        {
            m_dir.y = Random.Range(transform.position.y + (m_distanceToFloor / 2), transform.position.y + m_distanceToFloor);
            if(m_distanceToFloor<= m_MinDistanceToCollision)
            {
                m_dir.y = transform.position.y - m_MinDistanceToCollision;
            }
        }
        else
        {
            m_dir.y = Random.Range(transform.position.y - (m_distanceToGround / 2), transform.position.y - m_distanceToGround);
            if (m_distanceToGround <= m_MinDistanceToCollision)
            {
                m_dir.y = transform.position.y + m_MinDistanceToCollision;
            }
        }*/
        Physics.Raycast(transform.position, Vector3.down, out l_hit, Mathf.Infinity, m_blackboardEnemies.m_CollisionLayerMask);
        if (l_hit.collider != null)
        {
           // Debug.DrawRay(transform.position, Vector3.down * l_hit.distance, Color.green);
            m_distanceToGround = l_hit.distance;
        }
        for (int i = 0; i < m_PointsForCHeckDistanceToFloor.Length; i++)
        {
            Physics.Raycast(m_PointsForCHeckDistanceToFloor[i].position, Vector3.up, out l_hit, Mathf.Infinity, m_blackboardEnemies.m_CollisionLayerMask);
            if (l_hit.collider != null)
            {
                if (i == 0)
                {
                    m_distanceToFloor = l_hit.distance;
                }
                else
                {
                    if(l_hit.distance < m_distanceToFloor)
                    {
                        m_distanceToFloor = l_hit.distance;
                        print(m_distanceToFloor + "i: "+ i);
                        
                    }
                }
                
            }
        }
        //m_dir.y = Random.Range(m_MinDistanceToCollision, m_distanceToFloor - m_MinDistanceToCollision);
        m_dir.y =  m_distanceToFloor / 2;
        if (m_distanceToGround < m_MinDistanceToCollision)
        {
            m_dir.y = m_MinDistanceToCollision;
        }
        m_dir = m_dir - transform.position;
        m_dir.Normalize();
    }
    public void Move()
    {
        m_blackboardEnemies.m_Rigibody.velocity = m_dir * m_FollowSpeed * Time.deltaTime;
    }
    enum States
    {
        FollowNav,
        Attractor,
        ExitAttractor
    }
}
