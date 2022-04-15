using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
public class EnemieMovementFSM : FSM_AI
{

    FSM<States> m_brain;

    public BlackboardEnemies m_blackboardEnemies;
    float m_distanceToPlayer;
   
    public float m_Speed = 10f;
    public States m_CurrentState;
    HealthSystem m_hp;

    private void OnEnable()
    {
        m_hp.OnHit += OnHit;
    }
    private void OnDisable()
    {
        m_hp.OnHit -= OnHit;
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
        m_distanceToPlayer = Vector3.Distance(m_blackboardEnemies.m_Player.position, transform.position);
        m_CurrentState = m_brain.currentState;

    }

    public override void Init()
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
        });
        m_brain.SetOnEnter(States.GOTO_PLAYER, () =>
        {
            m_NavMeshAgent.isStopped = false;


        });
        m_brain.SetOnStay(States.GOTO_PLAYER, () =>
         {
             //TODO
             //OnDamageTaker => setPosition
             if (m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
             {

                 if (m_distanceToPlayer > m_blackboardEnemies.m_RangeToNear &&//Distancia ideal
                 m_distanceToPlayer <= m_blackboardEnemies.m_IdealRangeAttack)
                 {

                 }
             }
             if (m_distanceToPlayer > m_blackboardEnemies.m_RangeAttack)
             {
                 GoToPlayer();
             }
             else if (m_distanceToPlayer < m_blackboardEnemies.m_RangeToNear)
             {
                 GetAwayFromPlayer();
             }
             else if (m_distanceToPlayer < m_blackboardEnemies.m_IdealRangeAttack)
             {
                 StayAtIdealDistance();
             }

         });
        m_brain.SetOnStay(States.IDLE, () =>
        {
            if (m_distanceToPlayer > m_blackboardEnemies.m_RangeAttack)
            {
                m_brain.ChangeState(States.GOTO_PLAYER);
            }
        });
        //m_brain.SetOnEnter(()=> { });
        //m_brain.SetOnEnter(()=> { });
        //m_brain.SetOnEnter(()=> { });
        //m_brain.SetOnEnter(()=> { });

    }


    void GoToPlayer()
    {
        Vector3 l_DirectionToPlayer = m_blackboardEnemies.m_Player.position - transform.position;
        l_DirectionToPlayer.y = 0;
        l_DirectionToPlayer.Normalize();
        Vector3 l_Destination = m_blackboardEnemies.m_Player.position - l_DirectionToPlayer
            * Random.Range(m_blackboardEnemies.m_RangeAttack, m_blackboardEnemies.m_IdealRangeAttack);

        m_NavMeshAgent.destination = l_Destination;
    }
    void StayAtIdealDistance()
    {
        Vector3 l_DirectionToPlayer = m_blackboardEnemies.m_Player.position - transform.position;
        l_DirectionToPlayer.y = 0;
        l_DirectionToPlayer.Normalize();
        Vector3 l_Destination = l_DirectionToPlayer * (m_distanceToPlayer - m_blackboardEnemies.m_IdealRangeAttack) * Random.Range(1, m_blackboardEnemies.m_IdealRangeAttack);
    }
    void GetAwayFromPlayer()
    {
        Vector3 l_Direction = transform.position - m_blackboardEnemies.m_Player.position;
        l_Direction.y = 0;
        l_Direction.Normalize();
        Vector3 l_Destination = l_Direction * m_blackboardEnemies.m_IdealRangeAttack;

        m_NavMeshAgent.destination = l_Destination;
    }
    void CalculateNewPosAfterAttack()
    {
        float l_Angle = Mathf.Acos(m_distanceToPlayer / m_blackboardEnemies.m_MoveDistanceAfterAttack);

        Vector3 l_Destination = new Vector3(
           Mathf.Cos(l_Angle) * m_blackboardEnemies.m_MoveDistanceAfterAttack,
           0,
           Mathf.Sin(l_Angle) * m_blackboardEnemies.m_MoveDistanceAfterAttack)
          ;

       // print(l_Destination);
        l_Destination = transform.position - l_Destination ;//he añadido esto para que tenga en cuenta mi pos el desplazamiento, aun así van a puntos muy similares
        m_NavMeshAgent.destination = l_Destination;
    }
    public void OnHit() 
    {
        if(m_brain.currentState == States.GOTO_PLAYER)
        {
            CalculateNewPosAfterAttack();
        }else
        {
            m_brain.ChangeState(States.GOTO_PLAYER);
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
        IDLE,
        GOTO_PLAYER
    }
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
}
