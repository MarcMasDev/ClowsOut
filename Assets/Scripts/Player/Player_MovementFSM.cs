using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player_Blackboard))]
[RequireComponent(typeof(Player_MovementController))]
[RequireComponent(typeof(Player_InputHandle))]
public class Player_MovementFSM : MonoBehaviour
{
    #region Variables
    private enum MovementStates { INITIAL, IDLE, IDLE_AIMING, MOVING, MOVING_AIMING }
    private FSM<MovementStates> m_FSM;
    private float m_CurretVelocity;
    #endregion
    #region Components
    private Player_Blackboard m_Blackboard;
    private Player_MovementController m_Controller;
    private Player_InputHandle m_Input;
    #endregion
    void Awake()
    {
        m_Blackboard = GetComponent<Player_Blackboard>();
        m_Controller = GetComponent<Player_MovementController>();
        m_Input = GetComponent<Player_InputHandle>();

        //TODO: Maybe not here
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InitFSM();
    }

    //private void Update()
    //{
    //    //TODO: Sprint System (Lerp animator)
    //}

    public void Update()
    {
        m_FSM.Update();
    }

    private void InitFSM()
    {
        m_FSM = new FSM<MovementStates>(MovementStates.INITIAL);

        //ENTER
        m_FSM.SetOnEnter(MovementStates.IDLE, () =>
        {
            m_CurretVelocity = 0.0f;
        });
        m_FSM.SetOnEnter(MovementStates.IDLE_AIMING, () =>
        {
            m_CurretVelocity = 0.0f;
        });
        m_FSM.SetOnEnter(MovementStates.MOVING, () =>
        {
            m_CurretVelocity = m_Blackboard.m_WalkVelocity;
        });
        m_FSM.SetOnEnter(MovementStates.MOVING_AIMING, () =>
        {
            m_CurretVelocity = m_Blackboard.m_AimVelocity;
        });

        //UPDATE
        m_FSM.SetOnStay(MovementStates.INITIAL, () =>
        {
            m_FSM.ChangeState(MovementStates.IDLE);
        });

        m_FSM.SetOnStay(MovementStates.IDLE, () =>
        {
            if (m_Input.Moving)
            {
                m_FSM.ChangeState(MovementStates.MOVING);
            }

            m_Controller.GravityUpdate();
            m_Controller.SetMovement(m_CurretVelocity);
        });

        m_FSM.SetOnStay(MovementStates.IDLE_AIMING, () =>
        {
            if (m_Input.Moving)
            {
                m_FSM.ChangeState(MovementStates.MOVING_AIMING);
            }

            m_Controller.GravityUpdate();
            m_Controller.SetMovement(m_CurretVelocity);
        });

        m_FSM.SetOnStay(MovementStates.MOVING, () =>
        {
            if (!m_Input.Moving)
            {
                m_FSM.ChangeState(MovementStates.IDLE);
            }
            else if (m_Input.Aiming)
            {
                m_FSM.ChangeState(MovementStates.MOVING_AIMING);
            }

            m_Controller.MovementUpdate(m_Input.MovementAxis, m_Blackboard.m_Camera, m_Blackboard.m_LerpRotationPct);
            m_Controller.GravityUpdate();
            m_Controller.SetMovement(m_CurretVelocity);
        });

        m_FSM.SetOnStay(MovementStates.MOVING_AIMING, () =>
        {
            if (!m_Input.Moving)
            {
                m_FSM.ChangeState(MovementStates.IDLE_AIMING);
            }
            else if (!m_Input.Aiming)
            {
                m_FSM.ChangeState(MovementStates.MOVING);
            }

            m_Controller.MovementUpdate(m_Input.MovementAxis, m_Blackboard.m_Camera, m_Blackboard.m_LerpRotationPct);
            m_Controller.GravityUpdate();
            m_Controller.SetMovement(m_CurretVelocity);
        });
    }
}
