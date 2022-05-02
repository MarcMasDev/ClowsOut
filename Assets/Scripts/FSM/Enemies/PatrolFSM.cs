using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolFSM : FSM_AI
{
    private FSM<States> m_brain;
    public int m_index = 0;
    bool m_IsReturning = false;
    BlackboardEnemies m_blackboardEnemies;
    public float m_DistanceToWaypoint = 0f;
    public States m_CurrentState;
    float m_elapsedTime = 0f;
    public float m_TimeWaiting = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        m_blackboardEnemies = GetComponent<BlackboardEnemies>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        m_brain.Update();
        m_CurrentState = m_brain.currentState;
    }
    public override void Init()
    {
        base.Init();
        m_brain = new FSM<States>(States.INITIAL);
        m_brain.SetReEnter(() =>
        {
            m_index = 1;
            m_IsReturning = false;
            m_elapsedTime = 0f;
            m_brain.ChangeState(States.INITIAL);
        });
        m_brain.SetExit(() => {
            this.enabled = false;
        });
        m_brain.SetOnEnter(States.INITIAL, () => {
            m_brain.ChangeState(States.PATROL);
        });
        m_brain.SetOnStay(States.INITIAL, () => {
            m_brain.ChangeState(States.PATROL);
        });
        m_brain.SetOnEnter(States.PATROL, () => {
            m_DistanceToWaypoint = Vector3.Distance(m_blackboardEnemies.m_Waypoints[m_index].position   , transform.position);
            m_NavMeshAgent.isStopped = false;
            m_NavMeshAgent.destination = m_blackboardEnemies.m_Waypoints[m_index].position;
        });
        m_brain.SetOnStay(States.PATROL, () => {
            m_DistanceToWaypoint = Vector3.Distance(m_blackboardEnemies.m_Waypoints[m_index].position, transform.position);
            if(m_DistanceToWaypoint <= 2f)
            {
                NextWayPoint();
                m_brain.ChangeState(States.WAIT);
            }

        });
        m_brain.SetOnEnter(States.WAIT,()=>
        {
            m_elapsedTime = 0F;

        });
        m_brain.SetOnStay(States.WAIT,()=>
        {
            m_elapsedTime += Time.deltaTime;
            if(m_elapsedTime >= m_TimeWaiting)
            {
                m_NavMeshAgent.destination = m_blackboardEnemies.m_Waypoints[m_index].position;
                m_brain.ChangeState(States.PATROL);
            }

        });


    }
    public int NextWayPoint()
    {
        if (m_IsReturning)
        {
            m_index--;
            if (m_index <= 0)
            {
                m_IsReturning = false;
                m_index = 1;
            }
            return m_index;
        }
        else
        {
            m_index++;
            if ( m_index > m_blackboardEnemies.m_Waypoints.Length-1 )
            {
                m_IsReturning = true;
                m_index = m_blackboardEnemies.m_Waypoints.Length - 1;
            }
            return m_index;
        }
    }


    public override void ReEnter()
    {
       
        m_brain.ReEnter();
       
    }
    public override void Exit()
    {

        m_brain.Exit();
    }
    public enum States
    {
        INITIAL,
        PATROL,
        WAIT
    }
}
