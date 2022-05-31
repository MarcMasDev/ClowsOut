using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FSM : MonoBehaviour, IRestart
{
    #region Variables
    private enum LegsStates { INITIAL, MOVING, FALL, DASHING, DIE }
    private FSM<LegsStates> m_FSM;
    private float m_CurretVelocity;
    private float m_DashTimer;
    private float m_DashColdownTimer;
    private Vector3 m_InitalPos;
    private float m_AcumulatedPitchDelta;
    private Quaternion m_TargetRotation;
    private float m_FallTimer;
    public Action OnStartDashing, OnStopDashing;
    public Vector3 m_TargetForward;
    private float m_StopAimTimer;
    #endregion
    #region Components
    private Player_Blackboard m_Blackboard;
    private Player_MovementController m_Controller;
    private Player_InputHandle m_Input;
    private Player_ShootSystem m_ShootSystem;
    #endregion

    private void OnEnable()
    {
        m_ShootSystem.OnShoot += Shooted;
    }
    private void OnDisable()
    {
        m_ShootSystem.OnShoot -= Shooted;
    }
    void Awake()
    {
        m_Blackboard = GetComponent<Player_Blackboard>();
        m_Controller = GetComponent<Player_MovementController>();
        m_Input = GetComponent<Player_InputHandle>();
        m_ShootSystem = GetComponent<Player_ShootSystem>();
        m_Blackboard.m_DashTrail.SetActive(false);
        m_DashColdownTimer = m_Blackboard.m_DashColdownTime;
        m_Blackboard.m_Animator.SetBool("Ground", true);
        InitFSM();
    }
    private void Start()
    {
        AddRestartElement();
    }
    private void Update()
    {
        m_FSM.Update();
        if (m_StopAimTimer >= m_Blackboard.m_StopAimTime)
        {
            m_Blackboard.m_Animator.SetBool("StopAim", true);
        }
        m_StopAimTimer += Time.deltaTime;
    }

    private void InitFSM()
    {
        m_FSM = new FSM<LegsStates>(LegsStates.INITIAL);
        m_FSM.SetReEnter(() =>
        {
            m_FSM.ChangeState(LegsStates.INITIAL);
        });
        //ENTER
        m_FSM.SetOnEnter(LegsStates.DASHING, () =>
        {
            m_Blackboard.m_Animator.SetBool("Dash", true);
            OnStartDashing?.Invoke();
            print("HER I DASH WTF");
            m_Input.Aiming = false;
            m_CurretVelocity = m_Blackboard.m_DashVelocity;
            m_DashColdownTimer = 0.0f;
            m_DashTimer = 0.0f;
            m_Blackboard.m_DashTrail.SetActive(true);
            if (m_Input.Moving)
            {
                m_Controller.SetDashDirection(
                    GameManager.GetManager().GetCameraManager().m_Camera, 
                    m_Input.MovementAxis);
            }
            else
            {
                m_Controller.SetDashDirection(
                    GameManager.GetManager().GetCameraManager().m_Camera);
            }
        });
        m_FSM.SetOnEnter(LegsStates.FALL, () =>
        {
            m_Blackboard.m_Animator.SetBool("Ground", false);
        });
        //UPDATE
        m_FSM.SetOnStay(LegsStates.INITIAL, () =>
        {
            m_FSM.ChangeState(LegsStates.MOVING);
        });
        m_FSM.SetOnStay(LegsStates.MOVING, () =>
        {
            float l_AnimatorSpeedX = 0;
            float l_AnimatorSpeedZ = 0;

            if (m_Input.MovementAxis.x > 0)
            {
                l_AnimatorSpeedX = 1;
            }
            else if (m_Input.MovementAxis.x < 0)
            {
                l_AnimatorSpeedX = -1;
            }
            if (m_Input.MovementAxis.y > 0)
            {
                l_AnimatorSpeedZ = 1;
            }
            else if (m_Input.MovementAxis.y < 0)
            {
                l_AnimatorSpeedZ = -1;
            }

            l_AnimatorSpeedX = Mathf.Lerp(m_Blackboard.m_Animator.GetFloat("SpeedX"), l_AnimatorSpeedX, m_Blackboard.m_LerpAnimationMovementPct);
            l_AnimatorSpeedZ = Mathf.Lerp(m_Blackboard.m_Animator.GetFloat("SpeedZ"), l_AnimatorSpeedZ, m_Blackboard.m_LerpAnimationMovementPct);
            if (m_Input.Aiming)
            {
                m_Blackboard.m_Animator.SetBool("Aim", true);
                m_CurretVelocity = m_Blackboard.m_AimVelocity;
            }
            else
            {
                m_Blackboard.m_Animator.SetBool("Aim", false);
                m_CurretVelocity = m_Blackboard.m_WalkVelocity;
            }
            m_Blackboard.m_Animator.SetFloat("SpeedX", l_AnimatorSpeedX);
            m_Blackboard.m_Animator.SetFloat("SpeedZ", l_AnimatorSpeedZ);

            m_AcumulatedPitchDelta += m_Input.PitchDelta;

            if (m_Input.Moving)
            {
                m_TargetForward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
                m_TargetForward.y = 0;
                m_AcumulatedPitchDelta = 0;
            }
            else if (m_AcumulatedPitchDelta >= m_Blackboard.m_PitchToRotateRight)
            {
                m_TargetForward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
                m_TargetForward.y = 0;
                //m_Blackboard.m_Animator.SetTrigger("RotateRight");
                m_TargetRotation.eulerAngles = new Vector3(
                    0, transform.rotation.y + m_Blackboard.m_PitchToRotateRight, 0);
                m_AcumulatedPitchDelta = 0;
            }
            else if (m_AcumulatedPitchDelta <= -m_Blackboard.m_PitchToRotateLeft)
            {
                m_TargetForward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
                m_TargetForward.y = 0;
                //m_Blackboard.m_Animator.SetTrigger("RotateLeft");
                m_TargetRotation.eulerAngles = new Vector3(
                    0, transform.rotation.y + -m_Blackboard.m_PitchToRotateLeft, 0);
                m_AcumulatedPitchDelta = 0;
            }

            transform.forward = Vector3.Lerp(transform.forward, m_TargetForward, m_Blackboard.m_LerpRotationPct);

            //Quaternion newAngle = Quaternion.Lerp(
            //    transform.rotation, m_TargetRotation, m_Blackboard.m_LerpRotationPct);
            //newAngle.z = 0;
            //newAngle.x = 0;
            //transform.rotation = newAngle;

            m_Controller.MovementUpdate(m_Input.MovementAxis, 
                GameManager.GetManager().GetCameraManager().m_Camera);

            m_Controller.GravityUpdate();
            if (m_Input.Moving)
            {
                m_Controller.SetMovement(m_CurretVelocity);
            }
            else
            {
                m_Controller.SetMovement(0);
            }

            if (m_Input.Dashing)
            {
                if (m_DashColdownTimer >= m_Blackboard.m_DashColdownTime)
                {
                    m_FSM.ChangeState(LegsStates.DASHING);
                }
            }
            else if (!m_Controller.OnGround())
            {
                m_FSM.ChangeState(LegsStates.FALL);
            }
            //if (dead)
            m_DashColdownTimer += Time.deltaTime;
            m_Input.Dashing = false;
        });

        m_FSM.SetOnStay(LegsStates.DASHING, () =>
        {
            if (m_DashTimer < m_Blackboard.m_DashTime)
            {
                m_Controller.GravityUpdate();
                m_Controller.SetMovement(m_CurretVelocity);
            }
            else
            {
                m_FSM.ChangeState(LegsStates.MOVING);
                m_DashColdownTimer = 0;
            }
            m_DashTimer += Time.deltaTime;
        });

        m_FSM.SetOnStay(LegsStates.FALL, () =>
        {
            m_Controller.GravityUpdate();
            m_Controller.SetMovement(m_CurretVelocity);

            if (m_Controller.OnGround())
            {
                m_FSM.ChangeState(LegsStates.MOVING);
            }

            m_FallTimer += Time.deltaTime;
        });

        //EXIT
        m_FSM.SetOnExit(LegsStates.FALL, () =>
        {
            if (m_FallTimer >= m_Blackboard.m_TimeToLand)
            {
                m_Blackboard.m_Animator.SetTrigger("Land");
            }
            m_Blackboard.m_Animator.SetBool("Ground", true);
            m_FallTimer = 0;
        });
        m_FSM.SetOnExit(LegsStates.DASHING, () =>
        {
            m_Blackboard.m_Animator.SetBool("Dash", false);
            m_Blackboard.m_DashTrail.SetActive(false);
            OnStopDashing?.Invoke();
        });
    }
    public void AddRestartElement()
    {
        GameManager.GetManager().GetRestartManager().addRestartElement(this);
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
    private void Shooted()
    {
        m_Blackboard.m_Animator.SetBool("StopAim", false);
        m_StopAimTimer = 0;
    }
}
