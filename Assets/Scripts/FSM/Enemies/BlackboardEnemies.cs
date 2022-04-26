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
    public bool m_IsLinq = false;
    public float m_Height;
    public HighFSM.States m_PreviusState;
    public bool m_Pause = false;
    public float m_TimeToReactive = 2f;
    public Rigidbody m_Rigibody;
    //TODO: Take player from Gamecontroller
    private void Awake()
    {
        m_Rigibody = GetComponent<Rigidbody>();
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_distanceToPlayer = Vector3.Distance(m_Player.position, transform.position);
        m_Waypoints = m_ParentWaypoints.GetComponentsInChildren<Transform>();
    }
    public void SetIsLinq()
    {
        m_IsLinq = true;
        LinqSystem.m_Instance.AddLinqued(gameObject);
    }
    public bool SeesPlayerSimple()
    {
        Vector3 l_PlayerPosition = m_Player.position + Vector3.up * m_Height;
        Vector3 l_EyesEnemyPosition = transform.position + Vector3.up * m_Height;
        Vector3 l_Direction = l_PlayerPosition - l_EyesEnemyPosition;
        float l_DistanceToPlayer = l_Direction.magnitude;
        l_Direction /= l_DistanceToPlayer;
        Ray l_ray = new Ray(l_EyesEnemyPosition, l_Direction);
        if (!Physics.Raycast(l_ray, l_DistanceToPlayer, m_CollisionLayerMask.value))
        {
            Debug.DrawLine(l_EyesEnemyPosition, l_PlayerPosition, Color.red);
            return true;
        }
        Debug.DrawLine(l_EyesEnemyPosition, l_PlayerPosition, Color.magenta);
        return false;
    }
}
