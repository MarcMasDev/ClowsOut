using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighFSM : FSM_AI
{
    public FSM_AI m_MoveFSM;
    public FSM_AI m_AtackFSM;
    private FSM<States> m_brain;
    public States m_CurrentState;
    BlackboardEnemies m_blackboardEnemies;

    bool m_addedToTicketSystem = false;

    void Start()
    {
        m_blackboardEnemies = GetComponent<BlackboardEnemies>();
     

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        m_brain.Update();
        m_CurrentState = m_brain.currentState;
        m_blackboardEnemies.m_distanceToPlayer = Vector3.Distance(m_blackboardEnemies.m_Player.position, transform.position);
        if (!m_addedToTicketSystem)
        {
            if (m_blackboardEnemies.m_distanceToPlayer <= m_blackboardEnemies.m_RangeAttack)
            {
                m_addedToTicketSystem = true;
                TicketSystem.m_Instance.EnemyInRange(this);
            }
        }else if (m_blackboardEnemies.m_distanceToPlayer >= m_blackboardEnemies.m_RangeAttack)
        {

            TicketSystem.m_Instance.EnemyOutRange(this);
        }
        
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
        m_brain.SetOnEnter(States.INITIAL, () => {
            m_brain.ChangeState(States.MOVEFSM);
        }); 
        m_brain.SetOnEnter(States.MOVEFSM, () => {

            m_MoveFSM.enabled = true;
            m_MoveFSM.ReEnter();
        });
        m_brain.SetOnEnter(States.ATACKFSM, () => {
            Debug.Log("attack state enter");
            m_AtackFSM.enabled = true;
            m_AtackFSM.ReEnter();
        });

        m_brain.SetOnStay(States.INITIAL, () => {
            m_brain.ChangeState(States.MOVEFSM);
        });
        m_brain.SetOnStay(States.MOVEFSM, () => {

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
        Debug.Log("change to attack");
        m_brain.ChangeState(States.ATACKFSM);
    }
    public enum States
    {
        INITIAL ,
        MOVEFSM,
        ATACKFSM
    }
   
}
