using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighFSM : FSM_AI
{
    public FSM_AI m_MoveFSM;
    //public FSM_AI m_AtackFSM;
    private FSM<States> m_brain;
    public States m_CurrentState;
    void Start()
    {
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
        m_brain.SetOnStay(States.INITIAL, () => {
            m_brain.ChangeState(States.MOVEFSM);
        });


    }
    public void InvokeAttack()
    {
        m_brain.ChangeState(States.ATACKFSM);
    }
    public enum States
    {
        INITIAL,
        MOVEFSM,
        ATACKFSM
    }
}
