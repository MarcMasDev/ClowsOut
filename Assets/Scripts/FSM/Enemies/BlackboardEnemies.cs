using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackboardEnemies : MonoBehaviour
{
    [SerializeField] public FMODDogger m_FMODDogger;
    [Header("FSM info")]
    public float m_Height = 0.875f;//La mitad de la altura
    private HighFSM m_highFSM;
    public bool m_IsGrounded = true;
    public HighFSM.States m_PreviusState;
    public Transform m_AimTarget;
    public Animator m_Animator;
    [Header("Movement")]
        public float m_Speed = 5f;
    public Transform m_Player;
    [HideInInspector] public Transform m_PlayerAimPoint;
    public float m_RangeAttack = 15f;
    public float m_IdealRangeAttack = 10f;
    public float m_RangeToNear = 5f;
    public float m_MoveDistanceAfterAttack = 8f;
    public bool m_FinishAttack = false;
    [Header("Patrol")]
    public float m_distanceToPlayer;
    public Transform m_ParentWaypoints;
    public float m_DetectionDistance = 100f;
    public float m_AngleVision = 60f;
    public LayerMask m_CollisionLayerMask, m_CollisionWithEffect, m_CollisionLayerMaskToDetect;  
    public Transform[] m_Waypoints;
    public float m_AngleMovement = 20f;
    [Header("alter states")]
    public bool m_IsLinq = false;
    public bool m_Pause = false;
    [Header("Attractor")]
    public float m_TimeToReactive = 2f;
    public float m_DistanceToArriveThePoint = 2f;
    public float m_DistanceToStopAttractor = 2f;
    public float m_SpeedToStopAttractor = 10f;
    public Rigidbody m_Rigibody;
    public float m_SpeedAttractor = 200f;
    public Collider m_Collider;
    [Header("Bullets Optimization")]
    public HealthSystem m_hp;
    public  NavMeshAgent m_nav;
    public IceState m_IceState;
    public Vector3 m_AttractorCenter;
    public bool m_isIceState;
    public float m_DamageBullet=20f;
    public bool m_isShooting { get; internal set; }

    private void Awake()
    {
        m_Rigibody = GetComponent<Rigidbody>();
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_PlayerAimPoint = m_Player.GetComponent<Player_Blackboard>().m_EnemyAimPoint;
        m_distanceToPlayer = Vector3.Distance(m_Player.position, transform.position);
        //m_Waypoints = m_ParentWaypoints.GetComponentsInChildren<Transform>();
        m_hp = GetComponent<HealthSystem>();
        m_IceState = GetComponent<IceState>();
        m_highFSM = GetComponent<HighFSM>();

        if (!m_highFSM.m_ExternAgent)
            m_nav = GetComponent<NavMeshAgent>();

}

    //private void Start()
    //{
    //    m_Player = GameManager.GetManager().GetPlayer().transform;
    //}
    public void SetIsLinq()
    {
        if (!m_IsLinq)
        {
            m_IsLinq = true;
            LinqSystem.m_Instance.AddLinqued(this);
        }
    }
    public void RemoveLink()
    {
            m_IsLinq = false;
            //LinqSystem.m_Instance.Removed(this);
        
    }
    public bool SeesPlayerSimple()
    {
        Vector3 l_PlayerPosition = m_Player.position + Vector3.up * m_Height;
        Vector3 l_EyesEnemyPosition = transform.position + Vector3.up * m_Height;
        Vector3 l_Direction = l_PlayerPosition - l_EyesEnemyPosition;
        float l_DistanceToPlayer = l_Direction.magnitude;
        l_Direction /= l_DistanceToPlayer;
        Ray l_ray = new Ray(l_EyesEnemyPosition, l_Direction);
        if (!Physics.Raycast(l_ray, l_DistanceToPlayer, m_CollisionLayerMaskToDetect.value))
        {
            Debug.DrawLine(l_EyesEnemyPosition, l_PlayerPosition, Color.red);
            return true;
        }
        //Debug.DrawLine(l_EyesEnemyPosition, l_PlayerPosition, Color.magenta);
        return false;
    }
    public void ActivateAttractorEffect(Vector3 center)
    {

        // m_Pause = true;
        if (gameObject.tag != "Drone")
        {
            m_Collider.isTrigger = false;
            m_nav.enabled = false;
            m_Rigibody.isKinematic = false;
            m_AttractorCenter = center;
            m_highFSM.StartAttractor();
        }
        
        //m_Rigibody.velocity = center;
    }
}
