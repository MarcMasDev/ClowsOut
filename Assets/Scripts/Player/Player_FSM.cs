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
    private float m_TargetVelocity;
    private float m_DashTimer;
    private float m_DashColdownTimer;
    private Vector3 m_InitalPos;
    private float m_AcumulatedPitchDelta;
    private float m_AcumulatedYawDelta;
    private float m_FallTimer;
    public Vector3 m_TargetForward;
    private float m_StopAimTimer;
    private float m_TargetTorsoYaw;
    private float m_TargetTorsoPitch;
    private float m_CurrentTorsoPitch;
    private float m_CameraPitch;
    private float m_PreviousCameraPitch;
    private float m_RotateTimer;
    private float m_RotateTime = 0.75f;
    private bool m_Rotated;
    private float m_HardRotateTime = 1f;
    private float m_HardRotateTimer;
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
        m_Rotated = true;
        m_HardRotateTimer = m_HardRotateTime;
        InitFSM();
    }
    private void Start()
    {
        AddRestartElement();
        m_CameraPitch = GameManager.GetManager().GetCameraManager().m_Camera.transform.localEulerAngles.y;
        m_PreviousCameraPitch = m_CameraPitch;
        m_TargetForward = transform.forward;
        m_TargetForward.y = 0;
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
            //OnStartDashing?.Invoke();
            m_Input.Aiming = false;
            GameManager.GetManager().GetCanvasManager().HideReticle();
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
                if (m_Input.Moving)
                {
                    m_TargetVelocity = m_Blackboard.m_AimVelocity;
                }
                else
                {
                    m_TargetVelocity = 0;
                }
            }
            else
            {
                m_Blackboard.m_Animator.SetBool("Aim", false);
                if (m_Input.Moving)
                {
                    m_TargetVelocity = m_Blackboard.m_WalkVelocity;
                }
                else
                {
                    m_TargetVelocity = 0;
                }
            }
            m_CurretVelocity = Mathf.Lerp(m_CurretVelocity, m_TargetVelocity, m_Blackboard.m_LerpAnimationMovementPct);
            m_Blackboard.m_Animator.SetFloat("SpeedX", l_AnimatorSpeedX);
            m_Blackboard.m_Animator.SetFloat("SpeedZ", l_AnimatorSpeedZ);
            m_CameraPitch = GameManager.GetManager().GetCameraManager().m_Camera.transform.localEulerAngles.y;
            //if (m_CameraPitch > 360)
            //{
            //    m_CameraPitch -= 360;
            //}
            //else if (m_CameraPitch < 0)
            //{
            //    m_CameraPitch += 360;
            //}
            if (MathF.Abs(m_CameraPitch - m_PreviousCameraPitch) >= 350)
            {
                if (m_PreviousCameraPitch > m_CameraPitch)
                {
                    m_PreviousCameraPitch = 360 - m_PreviousCameraPitch;
                }
                else
                {
                    m_PreviousCameraPitch = 360 + m_PreviousCameraPitch;
                }
            }
            //if (m_Input.Moving)
            //{
            //    m_Blackboard.m_RigController.m_Moving = true;

            //    m_AcumulatedPitchDelta = 0;
            //    m_AcumulatedYawDelta = 0;
            //    m_TargetTorsoYaw = 0;
            //    m_TargetTorsoPitch = 0;

            //    m_PreviousCameraPitch = m_CameraPitch;

            //    m_TargetForward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
            //    m_TargetForward.y = 0;
            //    transform.forward = m_TargetForward;
            //}
            //else
            //{

            m_AcumulatedPitchDelta += m_CameraPitch - m_PreviousCameraPitch;
            m_AcumulatedYawDelta = -GameManager.GetManager().GetCameraManager().m_Camera.transform.localEulerAngles.x;
            if (m_AcumulatedYawDelta < -180)
            {
                m_AcumulatedYawDelta += 360;
            }

            m_TargetTorsoYaw = (m_AcumulatedYawDelta - m_Blackboard.m_MinYaw) / (m_Blackboard.m_MaxYaw - m_Blackboard.m_MinYaw) * (1 + 1) - 1;
            m_TargetTorsoPitch = (m_AcumulatedPitchDelta - m_Blackboard.m_PitchToRotateLeft) / (m_Blackboard.m_PitchToRotateRight - m_Blackboard.m_PitchToRotateLeft) * (1 + 1) - 1;

            LegRepositionUpdate();

            m_Blackboard.m_Animator.SetFloat("TorsoYaw", Mathf.Lerp(m_Blackboard.m_Animator.GetFloat("TorsoYaw"), m_TargetTorsoYaw, m_Blackboard.m_LerpAnimationAimPct));
            m_Blackboard.m_Animator.SetFloat("TorsoPitch", Mathf.Lerp(m_Blackboard.m_Animator.GetFloat("TorsoPitch"), m_TargetTorsoPitch, m_Blackboard.m_LerpAnimationAimPct));
            //}
            if (m_Controller.OnWall())
            {
                m_Blackboard.m_OnWall = true;
                m_Blackboard.m_RigController.m_Wall = true;
                m_Blackboard.m_Animator.SetBool("OnWall", true);
                GameManager.GetManager().GetCanvasManager().HideReticle();
            }
            else
            {
                m_Blackboard.m_OnWall = false;
                m_Blackboard.m_RigController.m_Wall = false;
                m_Blackboard.m_Animator.SetBool("OnWall", false);
                GameManager.GetManager().GetCanvasManager().ShowReticle();
            }

            m_Controller.MovementUpdate(m_Input.MovementAxis, GameManager.GetManager().GetCameraManager().m_Camera);
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
            m_PreviousCameraPitch = m_CameraPitch;
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
            m_Controller.JumpMovementUpdate(m_Input.MovementAxis, GameManager.GetManager().GetCameraManager().m_Camera);
            m_Controller.GravityUpdate();
            m_Controller.SetMovement(m_CurretVelocity);

            if (m_Controller.OnGround())
            {
                m_FSM.ChangeState(LegsStates.MOVING);
            }
            m_TargetForward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
            m_TargetForward.y = 0;

            transform.forward = Vector3.Lerp(transform.forward, m_TargetForward, m_Blackboard.m_LerpRotationPct * 4);

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
            GameManager.GetManager().GetCanvasManager().ShowReticle();
            //OnStopDashing?.Invoke();
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
    private void LegRepositionUpdate()
    {
        if (m_TargetTorsoPitch >= 0.8f)// && m_Rotated)
        {
            m_TargetForward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
            m_TargetForward.y = 0;
            m_RotateTime = 0.75f;
            if (!m_Input.Moving)
            {
                m_Blackboard.m_Animator.SetBool("RotateRight", true);
                m_RotateTime = 1.25f;
            }
            m_AcumulatedPitchDelta = 0;
            m_RotateTimer = 0;
            m_Rotated = false;
        }
        else if (m_TargetTorsoPitch <= -0.8f) //&& m_Rotated)
        {
            m_TargetForward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
            m_TargetForward.y = 0;
            m_RotateTime = 0.75f;
            if (!m_Input.Moving)
            {
                m_Blackboard.m_Animator.SetBool("RotateLeft", true);
                m_RotateTime = 1.25f;
            }
            m_AcumulatedPitchDelta = 0;
            m_RotateTimer = 0;
            m_Rotated = false;
        }

        if (Mathf.Abs(m_CameraPitch - m_PreviousCameraPitch) >= 15)
        {
            m_HardRotateTimer = 0;
        }
        if (m_HardRotateTime > m_HardRotateTimer)
        {
            Vector3 l_Forward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
            l_Forward.y = 0;
            transform.forward = l_Forward;
            m_Rotated = true;
        }
        else if (!m_Rotated)
        {
            transform.forward = Vector3.Lerp(transform.forward, m_TargetForward, m_RotateTimer / m_RotateTime);
            m_RotateTimer += Time.deltaTime;
            if (!m_Rotated && m_RotateTimer >= m_RotateTime)
            {
                m_Blackboard.m_Animator.SetBool("RotateRight", false);
                m_Blackboard.m_Animator.SetBool("RotateLeft", false);
                m_Rotated = true;
                transform.forward = m_TargetForward;
                m_RotateTimer = 0;
            }
        }
        m_HardRotateTimer += Time.deltaTime;

        if (m_Input.Moving)
        {
            m_Blackboard.m_Animator.SetBool("RotateRight", false);
            m_Blackboard.m_Animator.SetBool("RotateLeft", false);
        }
    }
}
