using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFSM : FSM_AI
{
    private FSM<States> m_brain;
    int m_index = 0;
    bool m_IsReturning = false;
    BlackboardEnemies m_blackboardEnemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Init()
    {
        base.Init();
        m_brain = new FSM<States>(States.INITIAL);
        m_brain.SetReEnter(() =>
        {
            m_index = 0;
            m_IsReturning = false;
        });
        m_brain.SetExit(() => {
            
        });
        m_brain.SetOnEnter(States.INITIAL, () => {
            m_brain.ChangeState(States.PATROL);
        });
        m_brain.SetOnStay(States.INITIAL, () => {
            m_brain.ChangeState(States.PATROL);
        });


    }
    public int NextWayPoint()
    {
        if (m_IsReturning)
        {
            m_index--;
            if (m_index < 0)
            {
                m_IsReturning = false;
                m_index = 1;
            }
            return m_index;
        }
        else
        {
            m_index++;
            if ( m_index > m_blackboardEnemies.m_Waypoints.Length)
            {
                m_IsReturning = true;
                m_index = m_blackboardEnemies.m_Waypoints.Length - 1;
            }
            return m_index;
        }
    }

    bool SeesPlayer()
    {
        Vector3 l_PlayerPosition = m_blackboardEnemies.m_Player.position + Vector3.up * 1.6f;
        Vector3 l_EyesDronePosition = transform.position + Vector3.up * 1.6f;
        Vector3 l_Direction = l_PlayerPosition - l_EyesDronePosition;
        float l_DistanceToPlayer = l_Direction.magnitude;
        l_Direction /= l_DistanceToPlayer;
        Ray l_ray = new Ray(l_EyesDronePosition, l_Direction);
        Vector3 l_forward = transform.forward;
        l_forward.y = 0;
        l_forward.Normalize();
        l_Direction.y = 0;
        l_Direction.Normalize();
        if (l_DistanceToPlayer < m_blackboardEnemies.m_DetectionDistance
            && Vector3.Dot(l_forward, l_Direction) >= Mathf.Cos(m_blackboardEnemies.m_AngleVision * 0.5f * Mathf.Deg2Rad))
        {
            if (!Physics.Raycast(l_ray, l_DistanceToPlayer,m_blackboardEnemies.m_CollisionLayerMask.value))
            {
                Debug.DrawLine(l_EyesDronePosition, l_PlayerPosition, Color.red);
                return true;
            }
        }
        Debug.DrawLine(l_EyesDronePosition, l_PlayerPosition, Color.blue);
        return false;


    }
    public override void ReEnter()
    {
        m_index = 0;
        m_brain.ReEnter();
        base.ReEnter();
    }
    public override void Exit()
    {

        m_brain.Exit();
        base.Exit();
    }
    public enum States
    {
        INITIAL,
        PATROL
    }
}
