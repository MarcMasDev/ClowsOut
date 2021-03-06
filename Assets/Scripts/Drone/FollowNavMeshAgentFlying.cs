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
    public States m_state;
    Vector3 m_dir = Vector3.zero;
    public  List<NodePath> m_path;
    private NodePath m_currentNode;
    float m_distanceToFloor, m_distanceToGround;
    [SerializeField]
    float m_MinDistanceToCollision = 5f;
    Vector3 m_GroundHit = Vector3.zero;
    Vector3 m_previusPos;
    [SerializeField]
    private float m_MinDistanceToMove =0.2f;
    NodePath m_lastNode;
    NodePath m_entrance;
    [SerializeField]
    private float m_ArriveDistance = 1f;
    AStarCreatePath m_aStarCreatePath;
    int m_index=0;
    [SerializeField] NodePath a, b;
    // Start is called before the first frame update
    void Start()
    {
        m_blackboardEnemies = GetComponent<BlackboardEnemies>();
        m_state = States.prueba;
        m_blackboardEnemies.m_Rigibody.isKinematic = false;
        m_previusPos = transform.position;
        m_aStarCreatePath = GetComponent<AStarCreatePath>();
        //
        m_entrance = a;
        m_path = m_aStarCreatePath.Inizialize(a, b);
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
            case States.StartPath:
                CheckCollisions();
                StartPath();
                Move();
                break;
            case States.FollowPath:
                CheckCollisions();
                FollowPath();
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
            case States.prueba:
                StartPath();
                Move();
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
        m_previusPos = transform.position;
    }
    public void CheckCollisions()
    {
        m_hits = new RaycastHit[m_directions.Length];
        bool l_Collision = false;
        bool l_First = false;
        for (int i = 0; i < m_directions.Length; i++)
        {
            //Ponemos la direcion en global
            Vector3 dir = transform.TransformDirection(m_directions[i]);
            Physics.Raycast(transform.position, dir, out m_hits[i], m_DistanceToCheck, m_blackboardEnemies.m_CollisionLayerMask);
            if (m_hits[i].collider != null)
            {
                l_Collision = true;
                if(l_Collision && !l_First)
                {
                    l_First = true;
                    m_dir = Vector3.zero;
                }
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
        Physics.Raycast(transform.position, Vector3.down, out l_hit, Mathf.Infinity, m_blackboardEnemies.m_CollisionLayerMask);
        if (l_hit.collider != null)
        {
           // Debug.DrawRay(transform.position, Vector3.down * l_hit.distance, Color.green);
            m_distanceToGround = l_hit.distance;
            m_GroundHit = l_hit.point;
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
        if(Vector3.Distance(transform.position, m_previusPos) == m_MinDistanceToMove || m_blackboardEnemies.m_nav.isStopped)
        {
            m_dir = Vector3.zero;
        }
    }
    public void StartPath()

    {
        m_index = 0;
        m_dir = m_entrance.transform.position - transform.position;
        m_dir.Normalize();
        if(Vector3.Distance(m_entrance.transform.position , transform.position) <= m_ArriveDistance)
        {
            m_dir = Vector3.zero;
            //m_path = m_aStarCreatePath.Inizialize(m_entrance, m_lastNode);
            m_currentNode = m_entrance;
            m_state = States.FollowPath;
        }
    } 
    public void FollowPath()
    {
        if (Vector3.Distance(m_path[m_index].transform.position, transform.position) <= m_ArriveDistance)
        {
            m_dir = Vector3.zero;
            m_currentNode = m_path[m_index];
            m_index += 1;
        }
        m_dir = m_path[m_index].transform.position - transform.position;
        m_dir.Normalize();

    }
    public void Move()
    {
        m_blackboardEnemies.m_Rigibody.velocity = m_dir * m_FollowSpeed * Time.deltaTime;
    }
    public enum States
    {
        FollowNav,
        Attractor,
        ExitAttractor,
        StartPath,
        FollowPath,
        ExitPath,
        prueba
    }
    public Vector3 GetGoundHitPoint()
    {
        return m_GroundHit;
    }
    public void SetStateToEnterUnderground(NodePath lastNode)
    {
        m_lastNode = lastNode;
        if (m_lastNode.m_IsAnEntrance)
        {
            m_entrance = m_lastNode;
            if (m_state == States.FollowPath)
            {
                m_state = States.FollowNav;
            }
            else
            {
                m_state = States.FollowPath;
            }
        }
    }
}
