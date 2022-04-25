using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackboardEnemies : MonoBehaviour
{
    public float m_Speed = 5f;
    public Transform m_Player;
    public float m_RangeAttack = 15f;
    public float m_IdealRangeAttack = 10f;
    public float m_RangeToNear = 5f;
    public float m_MoveDistanceAfterAttack = 8f;
    public bool m_FinishAttack = false;
    public float m_distanceToPlayer;
    public Transform m_ParentWaypoints;
    public float m_DetectionDistance = 100f;
    public float m_AngleVision = 60f;
    public LayerMask m_CollisionLayerMask;  
    public Transform[] m_Waypoints;
    public float m_AngleMovement = 20f;
    public float m_Height;
    public HighFSM.States m_PreviusState;
    
    //TODO: Take player from Gamecontroller
    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_distanceToPlayer = Vector3.Distance(m_Player.position, transform.position);
        m_Waypoints = m_ParentWaypoints.GetComponentsInChildren<Transform>();
    }
}
