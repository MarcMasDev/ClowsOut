using UnityEngine;
using UnityEngine.AI;
public class HighFSM : FSM_AI
{
    private static int ID;
    public int m_ID;
    public FSM_AI m_MoveFSM;
    public FSM_AI m_AtackFSM;
    public FSM_AI m_PatrolFSM;
    private FSM<States> m_brain;
    public States m_CurrentState;
    BlackboardEnemies m_blackboardEnemies;
    float m_Height = 1.6f;

    bool m_addedToTicketSystem = false;
    float m_timer = 0f;
    void Start()
    {
        m_ID = ID;
        ID++;
        
        m_blackboardEnemies = GetComponent<BlackboardEnemies>();
        Init();
        ChangeSpeed(m_blackboardEnemies.m_Speed);
        m_blackboardEnemies.m_Pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_blackboardEnemies.m_Pause)
        {
            m_brain.Update();
        }
        else
        {
            m_timer += Time.deltaTime;
            if(m_timer > m_blackboardEnemies.m_TimeToReactive)
            {
                m_timer = 0f;
                m_blackboardEnemies.m_Pause = false;
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
                m_blackboardEnemies.m_Rigibody.isKinematic = true;
            }
        }
        
        m_CurrentState = m_brain.currentState;
        m_blackboardEnemies.m_distanceToPlayer = Vector3.Distance(m_blackboardEnemies.m_Player.position, transform.position);
        if (!m_addedToTicketSystem)
        {
            if (m_blackboardEnemies.m_distanceToPlayer <= m_blackboardEnemies.m_RangeAttack && SeesPlayer())
            {
                m_addedToTicketSystem = true;
                TicketSystem.m_Instance.EnemyInRange(this);
            }
        }
        else if (m_blackboardEnemies.m_distanceToPlayer >= m_blackboardEnemies.m_RangeAttack && m_addedToTicketSystem)
        {
            TicketSystem.m_Instance.EnemyOutRange(this);
            m_addedToTicketSystem = false;
        }
        
    }
    public override void Init()
    {
        base.Init();
        m_brain = new FSM<States>(States.INITIAL);
        m_brain.SetReEnter(() =>
        {
            m_brain.ChangeState(States.INITIAL);
        });
        m_brain.SetExit(() =>
        {
            this.enabled = false;
        });
        m_brain.SetOnEnter(States.INITIAL, () => {
            m_brain.ChangeState(States.PATROL);
        }); 
        m_brain.SetOnEnter(States.MOVEFSM, () => {

            m_MoveFSM.enabled = true;
            m_MoveFSM.ReEnter();
            m_blackboardEnemies.m_FinishAttack = false;
        });
        m_brain.SetOnEnter(States.PATROL, () => {

            m_PatrolFSM.enabled = true;
            m_PatrolFSM.ReEnter();
          
        });
        m_brain.SetOnEnter(States.ATACKFSM, () => {
            m_AtackFSM.enabled = true;
            m_AtackFSM.ReEnter();

        });

        m_brain.SetOnStay(States.INITIAL, () => {
            m_brain.ChangeState(States.PATROL);
        });
        m_brain.SetOnStay(States.MOVEFSM, () => {

        });
        m_brain.SetOnStay(States.PATROL, () => {
            if (SeesPlayer() || m_blackboardEnemies.m_distanceToPlayer <= m_blackboardEnemies.m_IdealRangeAttack)
            {
                m_brain.ChangeState(States.MOVEFSM);
            }
        });
        m_brain.SetOnStay(States.ATACKFSM, () => {
            if (m_blackboardEnemies.m_FinishAttack)
            {
                m_brain.ChangeState(States.MOVEFSM);
                
            }
        });

        m_brain.SetOnExit(States.ATACKFSM, () => {
            m_AtackFSM.Exit();
            m_blackboardEnemies.m_FinishAttack = false;
           m_blackboardEnemies.m_PreviusState = States.ATACKFSM;
        });
        m_brain.SetOnExit(States.PATROL, () => {
            m_PatrolFSM.Exit();
            m_blackboardEnemies.m_PreviusState = States.PATROL;
        }); 
        m_brain.SetOnExit(States.MOVEFSM, () => {
            m_MoveFSM.Exit();
            m_blackboardEnemies.m_PreviusState = States.MOVEFSM;
        });


    }
    public override void ReEnter()
    {
        m_brain?.ReEnter();
    }
    public override void Exit()
    {
        m_brain?.Exit();
    }
    public void InvokeAttack()
    {
        m_brain.ChangeState(States.ATACKFSM);
    }
    public enum States
    {
        INITIAL ,
        MOVEFSM,
        PATROL,
        ATACKFSM
    }
    bool SeesPlayer()
    {
        Vector3 l_PlayerPosition = m_blackboardEnemies.m_Player.position + Vector3.up * m_Height;
        Vector3 l_EyesEnemyPosition = transform.position + Vector3.up * m_Height    ;
        Vector3 l_Direction = l_PlayerPosition - l_EyesEnemyPosition;
        float l_DistanceToPlayer = l_Direction.magnitude;
        l_Direction /= l_DistanceToPlayer;
        Ray l_ray = new Ray(l_EyesEnemyPosition, l_Direction);
        Vector3 l_forward = transform.forward;
        l_forward.y = 0;
        l_forward.Normalize();
        l_Direction.y = 0;
        l_Direction.Normalize();
        if (l_DistanceToPlayer < m_blackboardEnemies.m_DetectionDistance
            && Vector3.Dot(l_forward, l_Direction) >= Mathf.Cos(m_blackboardEnemies.m_AngleVision * 0.5f * Mathf.Deg2Rad))
        {
            if (!Physics.Raycast(l_ray, l_DistanceToPlayer, m_blackboardEnemies.m_CollisionLayerMask.value))
            {
                Debug.DrawLine(l_EyesEnemyPosition, l_PlayerPosition, Color.red);
                return true;
            }
        }
        Debug.DrawLine(l_EyesEnemyPosition, l_PlayerPosition, Color.blue);
        return false;


    }
}
