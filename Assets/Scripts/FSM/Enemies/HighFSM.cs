using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(GenericOnDeath))]
[RequireComponent(typeof(IceState))]

//TODO: Create a Parent with the Enemy an its
public class HighFSM : FSM_AI, IRestart
{
    private static int ID;
    public int m_ID;
    public FSM_AI m_MoveFSM;
    public FSM_AI m_AtackFSM;
    public FSM_AI m_PatrolFSM;
    private FSM<States> m_brain;
    public States m_CurrentState;
    BlackboardEnemies m_blackboardEnemies;

    bool m_addedToTicketSystem = false;
    float m_timer = 0f;
    Vector3 m_InitalPos;
    private CharacterController m_CharacterController;
    bool m_Fall = false;

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_ID = ID;
        ID++;
        m_blackboardEnemies = GetComponent<BlackboardEnemies>();
        AddRestartElement();
        Init();
        ChangeSpeed(m_blackboardEnemies.m_Speed);
        m_blackboardEnemies.m_Pause = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 l_Position = new Vector3(m_blackboardEnemies.m_Player.position.x, transform.position.y, m_blackboardEnemies.m_Player.position.z);
        m_blackboardEnemies.m_distanceToPlayer = Vector3.Distance(l_Position, transform.position);

        if (!m_blackboardEnemies.m_Pause)
        {
            m_brain.Update();
        }
        
        m_CurrentState = m_brain.currentState;
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
            m_timer = 0f;
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
        m_brain.SetOnEnter(States.ATTRACTOR, () => {
            m_blackboardEnemies.m_nav.enabled = false;
            m_blackboardEnemies.m_Rigibody.isKinematic = false;
            m_timer = 0f;
        }); 
        m_brain.SetOnStay(States.ATTRACTOR, () => {

            m_timer += Time.deltaTime;
            Vector3 l_Dir = m_blackboardEnemies.m_AttractorCenter - transform.position;
            l_Dir /= l_Dir.magnitude;
            Debug.DrawRay(m_blackboardEnemies.m_AttractorCenter, l_Dir,Color.green);
            l_Dir = l_Dir  * m_blackboardEnemies.m_SpeedAttractor * Time.deltaTime;
            if (!(m_blackboardEnemies.m_Rigibody.useGravity))//Si no estamos cayendo aplicamos esta velocidad
            {
                m_blackboardEnemies.m_Rigibody.velocity = l_Dir;
            }
            if (m_timer > m_blackboardEnemies.m_TimeToReactive)
            {
                m_Fall = true;
                m_blackboardEnemies.m_Rigibody.useGravity = true;
            }
            if (m_Fall)
            {
                if (m_blackboardEnemies.m_IsGrounded )//Colisiona con el suelo
                {
                    m_brain.ChangeState(m_blackboardEnemies.m_PreviusState);
                }
            }
        });

        m_brain.SetOnExit(States.ATTRACTOR, () => {
            m_blackboardEnemies.m_Rigibody.useGravity = false;
            m_Fall = false;
            m_blackboardEnemies.m_Pause = false;
            m_blackboardEnemies.m_nav.enabled = true;
            m_blackboardEnemies.m_nav.nextPosition = transform.position;
            m_blackboardEnemies.m_Rigibody.isKinematic = true;
            m_timer = 0f;
        });
        m_brain.SetOnStay(States.INITIAL, () => {
            m_brain.ChangeState(States.PATROL);
        });
        m_brain.SetOnStay(States.MOVEFSM, () => {

        });
        m_brain.SetOnStay(States.PATROL, () => {
            Vector3 l_Position = new Vector3(m_blackboardEnemies.m_Player.position.x, transform.position.y, m_blackboardEnemies.m_Player.position.z);
            m_blackboardEnemies.m_distanceToPlayer = Vector3.Distance(l_Position, transform.position);
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
        if(!(m_CurrentState == States.ATTRACTOR))
        {
            m_brain.ChangeState(States.ATACKFSM);
        }
    }
    public enum States
    {
        INITIAL ,
        MOVEFSM,
        PATROL,
        ATACKFSM,
        ATTRACTOR
    }
    public bool SeesPlayer()
    {
        Vector3 l_PlayerPosition = m_blackboardEnemies.m_Player.position + Vector3.up * m_blackboardEnemies.m_Height;
        Vector3 l_EyesEnemyPosition = transform.position + Vector3.up * m_blackboardEnemies.m_Height;
        Vector3 l_Direction = l_PlayerPosition - l_EyesEnemyPosition;
        float l_DistanceToPlayer = l_Direction.magnitude;
        l_Direction /= l_DistanceToPlayer;
        Vector3 l_forward = transform.forward;
        l_forward.Normalize();
        l_Direction.Normalize();
        Ray l_ray = new Ray(l_EyesEnemyPosition, l_Direction);


        if (m_blackboardEnemies.m_distanceToPlayer < m_blackboardEnemies.m_DetectionDistance
            && Vector3.Dot(l_forward, l_Direction) >= Mathf.Cos(m_blackboardEnemies.m_AngleVision * 0.5f * Mathf.Deg2Rad))
        {
            if (!Physics.Raycast(l_ray, l_DistanceToPlayer, m_blackboardEnemies.m_CollisionLayerMask.value))
            {
                return true;
            }
        }
        //Debug.DrawLine(l_EyesEnemyPosition, l_PlayerPosition, Color.red);
        return false;

        //Vector3 l_PlayerPosition = m_blackboardEnemies.m_Player.position + Vector3.up * m_Height;
        //Vector3 l_EyesEnemyPosition = transform.position + Vector3.up * m_Height    ;
        //Vector3 l_Direction = l_PlayerPosition - l_EyesEnemyPosition;
        //float l_DistanceToPlayer = l_Direction.magnitude;
        //l_Direction /= l_DistanceToPlayer;
        //Ray l_ray = new Ray(l_EyesEnemyPosition, l_Direction);
        //Vector3 l_forward = transform.forward;
        //l_forward.y = 0;
        //l_forward.Normalize();
        //l_Direction.y = 0;
        //l_Direction.Normalize();
        //Vector3 l_Position = new Vector3(m_blackboardEnemies.m_Player.position.x, transform.position.y, m_blackboardEnemies.m_Player.position.z);
        //m_blackboardEnemies.m_distanceToPlayer = Vector3.Distance(l_Position, transform.position);

        //Debug.Log("Enter Ticket 1: " + (m_blackboardEnemies.m_distanceToPlayer < m_blackboardEnemies.m_DetectionDistance));
        //Debug.Log("Enter Ticket 2: " + (Vector3.Dot(l_forward, l_Direction) >= Mathf.Cos(m_blackboardEnemies.m_AngleVision * 0.5f * Mathf.Deg2Rad)));
        //Debug.Log("Enter Ticket 3: " + (!Physics.Raycast(l_ray, l_DistanceToPlayer, m_blackboardEnemies.m_CollisionLayerMask.value)));

        //if (m_blackboardEnemies.m_distanceToPlayer < m_blackboardEnemies.m_DetectionDistance
        //    && Vector3.Dot(l_forward, l_Direction) >= Mathf.Cos(m_blackboardEnemies.m_AngleVision * 0.5f * Mathf.Deg2Rad))
        //{
        //    if (!Physics.Raycast(l_ray, l_DistanceToPlayer, m_blackboardEnemies.m_CollisionLayerMask.value))
        //    {
        //        Debug.DrawLine(l_EyesEnemyPosition, l_PlayerPosition, Color.green);
        //        return true;
        //    }
        //}
        //Debug.DrawLine(l_EyesEnemyPosition, l_PlayerPosition, Color.blue);
        //return false;


    }

    public void Restart()
    {
        gameObject.SetActive(true);
        transform.position = m_InitalPos;
        TicketSystem.m_Instance.EnemyOutRange(this);
        m_addedToTicketSystem = false;
        ChangeSpeed(m_blackboardEnemies.m_Speed);
        m_brain.ReEnter();
    }

    public void AddRestartElement()
    {
        m_InitalPos = transform.position;
        GameManager.GetManager().GetRestartManager().addRestartElement(this);
    }
    public void StartAttractor()
    {
        if (!(m_CurrentState.Equals(States.ATTRACTOR)))
        {
            m_brain.ChangeState(States.ATTRACTOR);
        }
        
    }
}
