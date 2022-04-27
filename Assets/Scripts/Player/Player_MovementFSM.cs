using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player_Blackboard))]
[RequireComponent(typeof(Player_MovementController))]
[RequireComponent(typeof(Player_InputHandle))]
public class Player_MovementFSM : MonoBehaviour, IRestart
{
    #region Variables
    private enum MovementStates { INITIAL, IDLE, IDLE_AIMING, MOVING, MOVING_AIMING, DASHING }
    private FSM<MovementStates> m_FSM;
    private float m_CurretVelocity;
    private float m_DashTimer;
    private float m_DashColdownTimer;
    private Vector3 m_InitalPos;
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
        m_Blackboard.m_DashTrail.SetActive(false);
        m_DashColdownTimer = m_Blackboard.m_DashColdownTime;
        InitFSM();
    }
    private void Start()
    {
        AddRestartElement();
    }

    //private void Update()
    //{
    //    //TODO: Sprint System (Lerp animator)
    //}

    public void Update()
    {
        //Debug.Log(m_FSM.currentState);
        m_FSM.Update();
        m_DashColdownTimer += Time.deltaTime;
    }

    private void InitFSM()
    {
        m_FSM = new FSM<MovementStates>(MovementStates.INITIAL);
        m_FSM.SetReEnter(() => 
        {
            m_DashColdownTimer += Time.deltaTime;
            m_FSM.ChangeState(MovementStates.INITIAL);
        });
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
        m_FSM.SetOnEnter(MovementStates.DASHING, () =>
        {
            m_CurretVelocity = m_Blackboard.m_DashVelocity;
            m_DashTimer = 0.0f;
            m_Blackboard.m_DashTrail.SetActive(true);
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
            else if (m_Input.Dashing)
            {
                if (m_DashColdownTimer >= m_Blackboard.m_DashColdownTime)
                {
                    m_Controller.SetDashDirection(CameraManager.Instance.m_Camera);
                    m_FSM.ChangeState(MovementStates.DASHING);
                }
                m_Input.Dashing = false;
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
            else if (m_Input.Dashing)
            {
                if (m_DashColdownTimer >= m_Blackboard.m_DashColdownTime)
                {
                    m_Controller.SetDashDirection(CameraManager.Instance.m_Camera, m_Input.MovementAxis);
                    m_FSM.ChangeState(MovementStates.DASHING);
                }
                m_Input.Dashing = false;
            }

            m_Controller.MovementUpdate(m_Input.MovementAxis, CameraManager.Instance.m_Camera, m_Blackboard.m_LerpRotationPct);
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

            m_Controller.MovementUpdate(m_Input.MovementAxis, CameraManager.Instance.m_Camera, m_Blackboard.m_LerpRotationPct);
            m_Controller.GravityUpdate();
            m_Controller.SetMovement(m_CurretVelocity);
        });
        m_FSM.SetOnStay(MovementStates.DASHING, () =>
        {
                if (m_DashTimer < m_Blackboard.m_DashTime)
                {
                    m_Controller.GravityUpdate();
                    m_Controller.SetMovement(m_CurretVelocity);
                }
                else
                {
                    m_FSM.ChangeState(MovementStates.MOVING);
                    m_DashColdownTimer = 0;
                }
                m_DashTimer += Time.deltaTime;
        });

        m_FSM.SetOnExit(MovementStates.DASHING, () =>
        {
            m_Blackboard.m_DashTrail.SetActive(false);
        });
    }

    public void AddRestartElement()
    {
        RestartElements.m_Instance.addRestartElement(this);
        m_InitalPos = transform.position;
    }

    public void Restart()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        transform.position = m_InitalPos;
        gameObject.SetActive(true);
        m_FSM.ReEnter();
    }
}
