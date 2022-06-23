using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(IceState))]

//TODO: Create a Parent with the Enemy an its
public class HighFSM : FSM_AI, IRestart
{
    private static int ID;
    public int m_ID;
    public FSM_AI m_MoveFSM;
    public FSM_AI m_AtackFSM;
    //public FSM_AI m_PatrolFSM;
    private FSM<States> m_brain;
    public States m_CurrentState;
    BlackboardEnemies m_blackboardEnemies;
    [SerializeField]
    bool m_addedToTicketSystem = false;
    float m_timer = 0f;
    Vector3 m_InitalPos;
    private CharacterController m_CharacterController;
    bool m_Fall = false;

    public NavMeshData m_NavMeshData;
   
    public Vector3 m_DoogerAnimateDirMovement = Vector3.zero;
    public Vector3 m_DoogerAnimateLookAtPos   = Vector3.zero;
    public bool m_DoogerAnimateIsAttacking    = false;
    public bool m_DoogerAnimateIsIce  = false;
    public bool m_DoogerAnimateDeath = false;

    public bool m_DoogerAnimateReciveDamage { get; private set; }

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
        if (gameObject.tag != "Drone")
        {
            m_blackboardEnemies.m_AimTarget.transform.parent = m_blackboardEnemies.m_PlayerAimPoint.transform;
            m_blackboardEnemies.m_AimTarget.transform.localPosition = Vector3.zero;
        }
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
            if (m_blackboardEnemies.m_distanceToPlayer < m_blackboardEnemies.m_RangeAttack && m_blackboardEnemies.SeesPlayerSimple())
            {
                m_addedToTicketSystem = true;
                TicketSystem.m_Instance.EnemyInRange(this);
            }
        }
        else if ((m_blackboardEnemies.m_distanceToPlayer > m_blackboardEnemies.m_RangeAttack) || 
            (!m_blackboardEnemies.SeesPlayerSimple()) && m_addedToTicketSystem)
        {
            TicketSystem.m_Instance.EnemyOutRange(this);
            m_addedToTicketSystem = false;
        }
        //A_Dogger
        if (gameObject.tag != "Drone")
        {
            m_DoogerAnimateDirMovement = m_blackboardEnemies.m_nav.velocity.normalized;
            ChangeSpeed(m_blackboardEnemies.m_Speed);

            float l_MaximX = Mathf.Round(m_DoogerAnimateDirMovement.x);
            float l_MaximZ = Mathf.Round(m_DoogerAnimateDirMovement.z);

            m_blackboardEnemies.m_Animator.SetFloat("SpeedX", Mathf.Lerp(m_blackboardEnemies.m_Animator.GetFloat("SpeedX"), l_MaximX, 0.5f));
            m_blackboardEnemies.m_Animator.SetFloat("SpeedZ", Mathf.Lerp(m_blackboardEnemies.m_Animator.GetFloat("SpeedZ"), l_MaximZ, 0.5f));

            m_DoogerAnimateLookAtPos = m_blackboardEnemies.m_PlayerAimPoint.transform.position;
            Vector3 l_forward = m_DoogerAnimateLookAtPos - transform.position;
            l_forward.y = 0;
            transform.forward = l_forward;

            //Vector3 l_look = m_DoogerAnimateLookAtPos - transform.position;
            //float l_Yaw = Vector3.Angle(l_look, transform.forward);
            //if (l_look.y < 0)
            //{
            //    l_Yaw = -l_Yaw;
            //}
            //if (l_Yaw < 180)
            //{
            //    l_Yaw += 360;
            //}
            //l_Yaw = -(l_Yaw - 360f);
            //Debug.Log("l_Yaw: " + l_Yaw);
            //float l_AnimYaw = (l_Yaw - (-90)) / (90 - (-90)) * (1 + 1) - 1;
            //m_blackboardEnemies.m_Animator.SetFloat("Yaw", l_AnimYaw);

            m_DoogerAnimateIsAttacking = m_blackboardEnemies.m_isShooting;
            if (m_DoogerAnimateIsAttacking)
            {
                m_blackboardEnemies.m_Animator.SetTrigger("Shoot");
            }

            m_DoogerAnimateIsIce = m_blackboardEnemies.m_isIceState;
            if (m_DoogerAnimateIsIce)
            {
                m_blackboardEnemies.m_Animator.speed = 0.25f;
            }
            else
            {
                m_blackboardEnemies.m_Animator.speed = 1f;
            }

            if (m_blackboardEnemies.m_isShooting)
            {
                m_blackboardEnemies.m_isShooting = false;
            }

            if (!m_blackboardEnemies.m_IsGrounded)
            {
                m_blackboardEnemies.m_Animator.SetBool("Fall", true);
            }
            else
            {
                m_blackboardEnemies.m_Animator.SetBool("Fall", false);
            }

            m_DoogerAnimateReciveDamage = m_blackboardEnemies.m_hp.m_reciveDamage;
            if (m_blackboardEnemies.m_hp.m_reciveDamage)
            {
                int hit = Random.Range(0, 3);
                m_blackboardEnemies.m_FMODDogger.Hit();
                switch (hit)
                {
                    case 0:
                        m_blackboardEnemies.m_Animator.SetTrigger("Hit0");
                        break;
                    case 1:
                        m_blackboardEnemies.m_Animator.SetTrigger("Hit1");
                        break;
                    case 2:
                        m_blackboardEnemies.m_Animator.SetTrigger("Hit2");
                        break;
                }
                m_blackboardEnemies.m_hp.m_reciveDamage = false;
            }

            m_DoogerAnimateDeath = m_blackboardEnemies.m_hp.m_Dead;
            if (m_DoogerAnimateDeath)
            {
                m_blackboardEnemies.m_Animator.SetTrigger("Die");
            }
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
            m_brain.ChangeState(States.MOVEFSM);
        }); 
        m_brain.SetOnEnter(States.MOVEFSM, () => {

            m_MoveFSM.enabled = true;
            m_MoveFSM.ReEnter();
            m_blackboardEnemies.m_FinishAttack = false;
        });
        //m_brain.SetOnEnter(States.PATROL, () => {

        //    m_PatrolFSM.enabled = true;
        //    m_PatrolFSM.ReEnter();
          
        //});
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
            float l_distance = l_Dir.magnitude;
            l_Dir /= l_Dir.magnitude;
            Debug.DrawRay(m_blackboardEnemies.m_AttractorCenter, l_Dir,Color.green);
            l_Dir = l_Dir  * m_blackboardEnemies.m_SpeedAttractor * Time.deltaTime;
            if (
                (!m_blackboardEnemies.m_Rigibody.useGravity)
            &&
            l_distance > m_blackboardEnemies.m_DistanceToStopAttractor)//Si no estamos cayendo aplicamos esta velocidad
            {
                m_blackboardEnemies.m_Rigibody.velocity = l_Dir;
            }
            print("velocity "+m_blackboardEnemies.m_Rigibody.velocity.magnitude +" "+ gameObject.name);
            if (!m_Fall &&
                 (m_timer > m_blackboardEnemies.m_TimeToReactive ))
            {
                print("stop attractor");
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
            m_blackboardEnemies.m_Collider.isTrigger = true;
            m_Fall = false;
            m_blackboardEnemies.m_Pause = false;
            m_blackboardEnemies.m_nav.enabled = true;
            m_blackboardEnemies.m_nav.nextPosition = transform.position;
            m_blackboardEnemies.m_Rigibody.isKinematic = true;
            m_timer = 0f;
        });
        m_brain.SetOnStay(States.INITIAL, () => {
            m_brain.ChangeState(States.MOVEFSM);
        });
        m_brain.SetOnStay(States.MOVEFSM, () => {

        });
        //m_brain.SetOnStay(States.PATROL, () => {
        //    Vector3 l_Position = new Vector3(m_blackboardEnemies.m_Player.position.x, transform.position.y, m_blackboardEnemies.m_Player.position.z);
        //    m_blackboardEnemies.m_distanceToPlayer = Vector3.Distance(l_Position, transform.position);
        //    if (SeesPlayer() || m_blackboardEnemies.m_distanceToPlayer <= m_blackboardEnemies.m_IdealRangeAttack)
        //    {
        //        m_brain.ChangeState(States.MOVEFSM);
        //    }
        //});
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
        //m_brain.SetOnExit(States.PATROL, () => {
        //    m_PatrolFSM.Exit();
        //    m_blackboardEnemies.m_PreviusState = States.PATROL;
        //}); 
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
        //PATROL,
        ATACKFSM,
        ATTRACTOR
    }
    //public bool SeesPlayer()
    //{
    //    Vector3 l_PlayerPosition = m_blackboardEnemies.m_Player.position + Vector3.up * m_blackboardEnemies.m_Height;
    //    Vector3 l_EyesEnemyPosition = transform.position + Vector3.up * m_blackboardEnemies.m_Height;
    //    Vector3 l_Direction = l_PlayerPosition - l_EyesEnemyPosition;
    //    float l_DistanceToPlayer = l_Direction.magnitude;
    //    l_Direction /= l_DistanceToPlayer;
    //    Vector3 l_forward = transform.forward;
    //    l_forward.Normalize();
    //    l_Direction.Normalize();
    //    Ray l_ray = new Ray(l_EyesEnemyPosition, l_Direction);


    //    if (m_blackboardEnemies.m_distanceToPlayer < m_blackboardEnemies.m_DetectionDistance
    //        && Vector3.Dot(l_forward, l_Direction) >= Mathf.Cos(m_blackboardEnemies.m_AngleVision * 0.5f * Mathf.Deg2Rad))
    //    {
    //        if (!Physics.Raycast(l_ray, l_DistanceToPlayer, m_blackboardEnemies.m_CollisionLayerMask.value))
    //        {
    //            return true;
    //        }
    //    }
    //    return false;

    //    Vector3 l_PlayerPosition = m_blackboardEnemies.m_Player.position + Vector3.up * m_Height;
    //    Vector3 l_EyesEnemyPosition = transform.position + Vector3.up * m_Height;
    //    Vector3 l_Direction = l_PlayerPosition - l_EyesEnemyPosition;
    //    float l_DistanceToPlayer = l_Direction.magnitude;
    //    l_Direction /= l_DistanceToPlayer;
    //    Ray l_ray = new Ray(l_EyesEnemyPosition, l_Direction);
    //    Vector3 l_forward = transform.forward;
    //    l_forward.y = 0;
    //    l_forward.Normalize();
    //    l_Direction.y = 0;
    //    l_Direction.Normalize();
    //    Vector3 l_Position = new Vector3(m_blackboardEnemies.m_Player.position.x, transform.position.y, m_blackboardEnemies.m_Player.position.z);
    //    m_blackboardEnemies.m_distanceToPlayer = Vector3.Distance(l_Position, transform.position);

    //    Debug.Log("Enter Ticket 1: " + (m_blackboardEnemies.m_distanceToPlayer < m_blackboardEnemies.m_DetectionDistance));
    //    Debug.Log("Enter Ticket 2: " + (Vector3.Dot(l_forward, l_Direction) >= Mathf.Cos(m_blackboardEnemies.m_AngleVision * 0.5f * Mathf.Deg2Rad)));
    //    Debug.Log("Enter Ticket 3: " + (!Physics.Raycast(l_ray, l_DistanceToPlayer, m_blackboardEnemies.m_CollisionLayerMask.value)));

    //    if (m_blackboardEnemies.m_distanceToPlayer < m_blackboardEnemies.m_DetectionDistance
    //        && Vector3.Dot(l_forward, l_Direction) >= Mathf.Cos(m_blackboardEnemies.m_AngleVision * 0.5f * Mathf.Deg2Rad))
    //    {
    //        if (!Physics.Raycast(l_ray, l_DistanceToPlayer, m_blackboardEnemies.m_CollisionLayerMask.value))
    //        {
    //            Debug.DrawLine(l_EyesEnemyPosition, l_PlayerPosition, Color.green);
    //            return true;
    //        }
    //    }
    //    Debug.DrawLine(l_EyesEnemyPosition, l_PlayerPosition, Color.blue);
    //    return false;
    //}

    public void Restart()
    {/*
        gameObject.SetActive(true);
        transform.position = m_InitalPos;
        TicketSystem.m_Instance.EnemyOutRange(this);
        m_addedToTicketSystem = false;
        ChangeSpeed(m_blackboardEnemies.m_Speed);
        m_brain.ReEnter();*/
        //m_brain.Exit();
        if (m_ExternAgent)
        {
            m_NavMeshAgent.gameObject.SetActive(false);
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //gameObject.SetActive(false);
        
    }

    public void AddRestartElement()
    {
        m_InitalPos = transform.position;
        GameManager.GetManager().GetRestartManager().addRestartElement(this,transform);
    }
    public void StartAttractor()
    {
        if (!(m_CurrentState.Equals(States.ATTRACTOR)))
        {
            m_brain.ChangeState(States.ATTRACTOR);
        }
        
    }
}
