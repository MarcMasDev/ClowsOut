using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemieMovementFSM : MonoBehaviour
{
    private FSM<States> m_brain;
    public BlackboardEnemies blackboardEnemies;
    float m_distanceToPlayer;
    NavMeshAgent m_NavMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_brain.Update();
        m_distanceToPlayer = Vector3.Distance(blackboardEnemies.m_Player.position , transform.position);
    }

    public void Init()
    {
        m_brain = new FSM<States>(States.INITIAL);
        m_brain.SetReEnter(() =>
        {
            m_brain.ChangeState(States.INITIAL);
        }); 
        m_brain.SetExit(() =>
        {
            this.enabled = false;
        });
        m_brain.SetOnEnter(States.IDLE,()=> {
            m_NavMeshAgent.isStopped = true;
        });
        m_brain.SetOnEnter(States.GOTO_PLAYER,()=> {
            m_NavMeshAgent.isStopped = false;
            if (m_distanceToPlayer > blackboardEnemies.m_RangeAttack)
            {
                GoToPlayer();
            }
            if(m_distanceToPlayer < blackboardEnemies.m_RangeToNear)
            {
                GetAwayFromPlayer();
            }

        });
        m_brain.SetOnStay(States.GOTO_PLAYER,()=> 
        {
            //OnDamageTaker => setPosition
            if (!m_NavMeshAgent.hasPath && m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
            {
               if(m_distanceToPlayer > blackboardEnemies.m_RangeToNear &&//Distancia ideal
                m_distanceToPlayer <= blackboardEnemies.m_IdealRangeAttack)
                {

                }

            }
        });
        //m_brain.SetOnEnter(()=> { });
        //m_brain.SetOnEnter(()=> { });
        //m_brain.SetOnEnter(()=> { });
        //m_brain.SetOnEnter(()=> { });

    }


    void GoToPlayer()
    {
        Vector3 l_DirectionToPlayer = blackboardEnemies.m_Player.position - transform.position;
        l_DirectionToPlayer.y = 0;
        l_DirectionToPlayer.Normalize();
        Vector3 l_Destination = blackboardEnemies.m_Player.position - l_DirectionToPlayer 
            * Random.Range(blackboardEnemies.m_RangeAttack, blackboardEnemies.m_IdealRangeAttack);

        m_NavMeshAgent.destination = l_Destination;
    }
    void GetAwayFromPlayer()
    {
        Vector3 l_Direction = transform.position - blackboardEnemies.m_Player.position;
        l_Direction.y = 0;
        l_Direction.Normalize();
        Vector3 l_Destination = l_Direction * blackboardEnemies.m_IdealRangeAttack;

        m_NavMeshAgent.destination = l_Destination;
    }
    void CalculateNewPosAfterAttack()
    {
       float l_Angle = Mathf.Acos(m_distanceToPlayer / blackboardEnemies.m_MoveDistanceAfterAttack);
        Vector3 l_Destination = new Vector3(
            Mathf.Cos(l_Angle) * blackboardEnemies.m_MoveDistanceAfterAttack,
            Mathf.Sin(l_Angle) * blackboardEnemies.m_MoveDistanceAfterAttack
            ,
            0);
        m_NavMeshAgent.destination = l_Destination;
    }
    public enum States
    {
        INITIAL,
        IDLE,
        GOTO_PLAYER
    }
}
