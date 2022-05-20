using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using System;

public class EnemieMovementFSM : FSM_AI
{

    FSM<States> m_brain;

    public BlackboardEnemies m_blackboardEnemies;
    
    public float m_Speed = 10f;
    public States m_CurrentState;
    HealthSystem m_hp;
    float m_Timer = 0f;
    [SerializeField]
    float m_MaxTimeIndle = 1f;

    private void OnEnable()
    {
        m_hp.m_OnHit += OnHit;
        m_hp.m_OnHit += OnHit;
    }
    private void OnDisable()
    {
        m_hp.m_OnHit -= OnHit;
    }
    // Start is called before the first frame update
    void Awake()
    {
        m_hp = GetComponent<HealthSystem>();
        base.Init();
        ChangeSpeed(m_Speed);
        m_blackboardEnemies = GetComponent<BlackboardEnemies>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        m_brain.Update();
        
        m_CurrentState = m_brain.currentState;

        transform.LookAt(m_blackboardEnemies.m_Player);
    }

    public override void Init()
    {
        m_brain = new FSM<States>(States.INITIAL);
        m_brain.SetReEnter(() =>
        {
            if (m_blackboardEnemies.m_PreviusState == HighFSM.States.ATACKFSM)
            {
                m_brain.ChangeState(States.GOTO_POSITION_AFTER_ATTACK);
            }
            else 
            {
                m_brain.ChangeState(States.INITIAL);
            }
               
        });
        m_brain.SetExit(() =>
        {
           
            this.enabled = false;
        });
        m_brain.SetOnEnter(States.INITIAL, () =>
        {
            m_brain.ChangeState(States.IDLE);
        });
        m_brain.SetOnStay(States.INITIAL, () =>
        {
            m_brain.ChangeState(States.IDLE);
        });
        m_brain.SetOnEnter(States.IDLE, () =>
        {
            m_NavMeshAgent.isStopped = true;
            m_Timer = 0f;
        });
        m_brain.SetOnEnter(States.GOTO_PLAYER, () =>
        {
            m_NavMeshAgent.isStopped = false;


        });
        m_brain.SetOnEnter(States.GOTO_POSITION_AFTER_ATTACK, () =>
        {
            m_NavMeshAgent.isStopped = false;
            FindPathAfterAtack();

        });
        m_brain.SetOnStay(States.GOTO_POSITION_AFTER_ATTACK, () =>
        {
            if (!m_NavMeshAgent.pathPending)
            {
                if (m_NavMeshAgent.remainingDistance <= m_NavMeshAgent.stoppingDistance)
                {
                    if (!m_NavMeshAgent.hasPath || m_NavMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        // Done
                        m_brain.ChangeState(States.GOTO_PLAYER);
                    }
                }
            }

        });
        m_brain.SetOnStay(States.GOTO_PLAYER, () =>
         {
             if (m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
             {

                 if (m_blackboardEnemies.m_distanceToPlayer > m_blackboardEnemies.m_RangeToNear &&//Distancia ideal
                 m_blackboardEnemies.m_distanceToPlayer <= m_blackboardEnemies.m_IdealRangeAttack)
                 {
                     Debug.Log("TODO");
                 }
             }
             if (m_blackboardEnemies.m_distanceToPlayer > m_blackboardEnemies.m_RangeAttack)
             {
                 GoToPlayer();
             }
             else if (m_blackboardEnemies.m_distanceToPlayer < m_blackboardEnemies.m_RangeToNear)
             {
                 GetAwayFromPlayer();
             }
             else if (m_blackboardEnemies.SeesPlayerSimple() && 
                m_blackboardEnemies.m_distanceToPlayer < m_blackboardEnemies.m_RangeAttack 
                && m_blackboardEnemies.m_distanceToPlayer > m_blackboardEnemies.m_RangeToNear)
             {
                 m_brain.ChangeState(States.IDLE);
             }
             else if(!m_blackboardEnemies.SeesPlayerSimple())
             {
                 
                 m_NavMeshAgent.destination = m_blackboardEnemies.m_Player.position;
             }
             //else if (m_blackboardEnemies.m_distanceToPlayer < m_blackboardEnemies.m_IdealRangeAttack)
             //{
             //    StayAtIdealDistance();
             //}

         });
        m_brain.SetOnStay(States.IDLE, () =>
        {
            m_Timer += Time.deltaTime;
            if (m_blackboardEnemies.m_distanceToPlayer > m_blackboardEnemies.m_RangeAttack ||
                !m_blackboardEnemies.SeesPlayerSimple())
            {
                m_brain.ChangeState(States.GOTO_PLAYER);
            }
            if (m_blackboardEnemies.m_distanceToPlayer < m_blackboardEnemies.m_RangeAttack && !m_blackboardEnemies.SeesPlayerSimple()) 
            {
                m_brain.ChangeState(States.GOTO_POSITION_AFTER_ATTACK);
            }
            if(m_Timer>= m_MaxTimeIndle)
            {
                m_brain.ChangeState(States.GOTO_POSITION_AFTER_ATTACK);
            }
        });

    }


    void GoToPlayer()
    {
        Vector3 l_DirectionToPlayer = m_blackboardEnemies.m_Player.position - transform.position;
        l_DirectionToPlayer.y = 0;
        l_DirectionToPlayer.Normalize();
        Vector3 l_Destination = m_blackboardEnemies.m_Player.position - l_DirectionToPlayer
            * UnityEngine.Random.Range(m_blackboardEnemies.m_RangeAttack, m_blackboardEnemies.m_IdealRangeAttack);

        l_Destination.y = transform.position.y;

        m_NavMeshAgent.destination = l_Destination;
    }
    void StayAtIdealDistance()
    {
        Vector3 l_DirectionToPlayer = m_blackboardEnemies.m_Player.position - transform.position;
        l_DirectionToPlayer.y = 0;
        l_DirectionToPlayer.Normalize();
        Vector3 l_Destination = l_DirectionToPlayer * (m_blackboardEnemies.m_distanceToPlayer - m_blackboardEnemies.m_IdealRangeAttack) * UnityEngine.Random.Range(1, m_blackboardEnemies.m_IdealRangeAttack);
    }
    void GetAwayFromPlayer()
    {
        Vector3 l_Direction = transform.position - m_blackboardEnemies.m_Player.position;
        l_Direction.y = 0;
        l_Direction.Normalize();
        Vector3 l_Destination = l_Direction * m_blackboardEnemies.m_IdealRangeAttack;

        m_NavMeshAgent.destination = l_Destination;
    }

    void FindPathAfterAtack()
    {
        float l_random = UnityEngine.Random.value;
        Vector3 l_desteny;
        l_desteny = RightLeftCalculate(l_random, m_blackboardEnemies.m_MoveDistanceAfterAttack, 0);
        if(l_desteny != Vector3.zero)
        {
            m_NavMeshAgent.destination = l_desteny;
        }
        else
        {
            m_brain.ChangeState(States.GOTO_PLAYER);
        }
    }
    Vector3 RightLeftCalculate(float random,float distanceToMove,int count)
    {
        if (count == 2)
        {
            distanceToMove = distanceToMove/2;
        }
        if (count >= 4)
        {
            return Vector3.zero;
        }
        Vector3 l_Destiny;
        if (random >= 0.5f)
        {
            l_Destiny = CalculateNewPosAfterAttack(true, distanceToMove, m_blackboardEnemies.m_AngleMovement);
        }
        else
        {
            l_Destiny = CalculateNewPosAfterAttack(false, distanceToMove, m_blackboardEnemies.m_AngleMovement);
        }
        if(l_Destiny == Vector3.zero)
        {
            return RightLeftCalculate((random + 0.5f)%1, distanceToMove, count+1);
        }
        return l_Destiny;
        
    }
    Vector3 CalculateNewPosAfterAttack(bool right, float moveDistance, float angle)
    {
        Vector3 l_PlayerPosition = m_blackboardEnemies.m_Player.transform.position;
        Vector3 l_DirEnemyToPlayer = (l_PlayerPosition - transform.position).normalized;

        angle *= (right ? 1f : -1f);
       
        Vector3 l_Direction = Quaternion.AngleAxis(angle, transform.up) * l_DirEnemyToPlayer;

        Vector3 l_Destination = l_Direction * moveDistance;

        l_Destination = transform.position + l_Destination;

       // Debug.DrawLine(transform.position, l_Destination, Color.red);
        NavMeshPath l_navmeshPath = new NavMeshPath();

        m_NavMeshAgent.CalculatePath(l_Destination, l_navmeshPath);
        if (l_navmeshPath.status == NavMeshPathStatus.PathComplete)
        {
            //Debug.DrawLine(transform.position, l_Destination, Color.red, 3);
            return l_Destination;
        }
        else
        {
            return Vector3.zero;
        }
    }

public void OnHit(float f) 
    {
        m_brain.ChangeState(States.GOTO_POSITION_AFTER_ATTACK);
    }
    public override void ReEnter()
    {
        m_brain?.ReEnter();
    }
    public override void Exit()
    {
        m_brain?.Exit();
    }
    public enum States
    {
        INITIAL,
        IDLE,
        GOTO_PLAYER,
        GOTO_POSITION_AFTER_ATTACK
    }
   /*
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(m_blackboardEnemies.m_Player.position, transform.up, m_blackboardEnemies.m_RangeAttack);
        Handles.Label(new Vector3(0, 0, 10), "m_RangeAttack green");
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(m_blackboardEnemies.m_Player.position, transform.up, m_blackboardEnemies.m_RangeToNear);
        Handles.Label(new Vector3(0, 0, 20), "m_RangeToNear yellow");
        UnityEditor.Handles.color = Color.magenta;
        UnityEditor.Handles.DrawWireDisc(m_blackboardEnemies.m_Player.position, transform.up, m_blackboardEnemies.m_IdealRangeAttack);
        Handles.Label(new Vector3(0, 0, 30), "m_IdealRangeAttack magenta");

    }
#endif
   */
}
