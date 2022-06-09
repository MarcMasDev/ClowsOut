using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player_FSM : MonoBehaviour, IRestart
{
    private enum PlayerStates { INITIAL, IDLE, AIM, SOFTAIM, AIM_WALK, SOFTAIM_RUN, RUN, DASHING, FALL, DEATH}
    #region Variables
    private FSM<PlayerStates> m_FSM;
    private Vector3 m_TargetForward;
    private Vector3 m_InitalPos;
    private float m_YawDelta;
    private float m_TargetVelocity;
    private float m_TargetYaw;
    private float m_TargetPitch;
    private float m_TargetAnimatorSpeedY;
    private float m_TargetAnimatorSpeedX;
    private float m_SoftAimTimer;
    private float m_RotateTimer;
    private float m_PreviousCameraPitch;
    private float m_PitchDelta;
    private float m_FallTimer;
    private float m_DashTimer;
    [HideInInspector]
    public float m_DashColdownTimer;
    private float m_CurrentSpeed;
    private bool m_Rotated;
    private float m_TargetAimWeight;
    public float m_LandTimer;
    private bool m_AnimationRotating;
    private Vector2 m_PreviousMoveInput;
    private Vector2 m_PreviousRunInput;
    private Vector3 m_RunDirection;
    private float m_PreviousYawDelta;
    private bool m_Land;
    #endregion

    private bool m_Dashing;
    private float m_SpeedTimer;
    private float m_MoveTimer;
    private float m_LookAtTimer;
    private float m_WeightTimer;
    private float m_AimTimer;

    #region Components
    private Player_Blackboard m_Blackboard;
    private Player_MovementController m_Controller;
    private Player_InputHandle m_Input;
    private Player_ShootSystem m_ShootSystem;
    private HealthSystem m_HealthSystem;
    #endregion

    private void OnEnable()
    {
        m_ShootSystem.OnShoot += Shooted;
        m_HealthSystem.m_OnHit += Hit;
        m_HealthSystem.m_OnDeath += Death;
    }
    private void OnDisable()
    {
        m_ShootSystem.OnShoot -= Shooted;
        m_HealthSystem.m_OnHit -= Hit;
        m_HealthSystem.m_OnDeath -= Death;
    }
    void Awake()
    {
        m_Blackboard = GetComponent<Player_Blackboard>();
        m_Controller = GetComponent<Player_MovementController>();
        m_Input = GetComponent<Player_InputHandle>();
        m_ShootSystem = GetComponent<Player_ShootSystem>();
        m_HealthSystem = GetComponent<HealthSystem>();
    }
    private void Start()
    {
        AddRestartElement();
        m_Blackboard.m_DashTrail.SetActive(false);
        m_DashColdownTimer = m_Blackboard.m_DashColdownTime;
        m_Blackboard.m_Animator.SetBool("Ground", true);
        m_Rotated = true;
        m_PreviousCameraPitch = GameManager.GetManager().GetCameraManager().m_Camera.transform.localEulerAngles.y;
        m_TargetForward = transform.forward;
        m_TargetForward.y = 0;
        m_SoftAimTimer = m_Blackboard.m_SoftAimTime;
        InitFSM();
    }
    private void Update()
    {
        if (m_Input.MovementAxis != m_PreviousMoveInput)
        {
            ResetOnChangeDir();
        }
        m_FSM.Update();
        m_PreviousMoveInput = m_Input.MovementAxis;
        if (m_Input.Aiming)
        {
            m_Blackboard.m_Animator.SetBool("Aim", true);
        }
        else
        {
            m_Blackboard.m_Animator.SetBool("Aim", false);
        }
        if (m_SoftAimTimer <= m_Blackboard.m_SoftAimTime)
        {
            m_Blackboard.m_Animator.SetBool("SoftAim", true);
        }
        else
        {
            m_Blackboard.m_Animator.SetBool("SoftAim", false);
        }
        m_SoftAimTimer += Time.deltaTime;
        m_DashColdownTimer += Time.deltaTime;
        m_Input.Dashing = false;
        //Stop FSM When dying

        Debug.Log(m_FSM.currentState);
        //Debug.Log(m_DashTimer);
    }
    private void InitFSM()
    {
        #region START
        m_FSM = new FSM<PlayerStates>(PlayerStates.INITIAL);
        m_FSM.SetReEnter(() =>
        {
            m_FSM.ChangeState(PlayerStates.INITIAL);
        });
        #endregion

        #region ENTER
        m_FSM.SetOnEnter(PlayerStates.IDLE, () =>
        {
            ResetOnEnterTimers();

            m_Blackboard.m_Animator.SetBool("OnWall", false);

            m_Blackboard.m_Animator.SetFloat("SpeedX", 0);
            m_Blackboard.m_Animator.SetFloat("SpeedY", 0);
            m_Blackboard.m_Animator.SetFloat("Speed", 0);

            m_TargetAimWeight = 0;
            m_TargetVelocity = 0;
            m_Blackboard.m_Animator.SetBool("Moving", false);

            m_CurrentSpeed = 0;
        });
        m_FSM.SetOnEnter(PlayerStates.AIM, () =>
        {
            ResetOnEnterTimers();

            m_Blackboard.m_Animator.SetFloat("SpeedX", 0);
            m_Blackboard.m_Animator.SetFloat("SpeedY", 0);
            m_Blackboard.m_Animator.SetFloat("Speed", 0);

            m_TargetAimWeight = 1;
            m_TargetVelocity = 0;
            m_Blackboard.m_Animator.SetBool("Moving", false);

            m_CurrentSpeed = 0;
        });
        m_FSM.SetOnEnter(PlayerStates.SOFTAIM, () =>
        {
            ResetOnEnterTimers();

            m_Blackboard.m_Animator.SetFloat("SpeedX", 0);
            m_Blackboard.m_Animator.SetFloat("SpeedY", 0);
            m_Blackboard.m_Animator.SetFloat("Speed", 0);

            //m_TargetAimWeight = 1;
            StartCoroutine(SetAimWeight(m_Blackboard.m_AimRig, 1));

            DeltaYawUpdate();
            m_TargetYaw = (m_YawDelta - m_Blackboard.m_MinYaw) / (m_Blackboard.m_MaxYaw - m_Blackboard.m_MinYaw) * (1 + 1) - 1;
            m_Blackboard.m_Animator.SetFloat("Yaw", m_TargetYaw);

            //DeltaPitchUpdateSoftAim();
            //m_TargetPitch = (m_PitchDelta - m_Blackboard.m_PitchToRotateLeft) /
            //    (m_Blackboard.m_PitchToRotateRight - m_Blackboard.m_PitchToRotateLeft) * (1 + 1) - 1;
            //m_Blackboard.m_Animator.SetFloat("Pitch", m_TargetPitch);

            m_TargetForward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
            m_TargetForward.y = 0;
            transform.forward = m_TargetForward;

            m_Blackboard.m_Animator.SetBool("Moving", false);

            m_CurrentSpeed = 0;
        });
        m_FSM.SetOnEnter(PlayerStates.AIM_WALK, () =>
        {
            ResetOnEnterTimers();

            m_TargetAimWeight = 1;
            m_TargetVelocity = m_Blackboard.m_WalkVelocity;
            m_Blackboard.m_Animator.SetBool("Moving", true);
        });
        m_FSM.SetOnEnter(PlayerStates.SOFTAIM_RUN, () =>
        {
            ResetOnEnterTimers();

            //m_TargetAimWeight = 1;
            StartCoroutine(SetAimWeight(m_Blackboard.m_AimRig, 1));

            DeltaYawUpdate();
            m_TargetYaw = (m_YawDelta - m_Blackboard.m_MinYaw) / (m_Blackboard.m_MaxYaw - m_Blackboard.m_MinYaw) * (1 + 1) - 1;
            m_Blackboard.m_Animator.SetFloat("Yaw", m_TargetYaw);

            //DeltaPitchUpdateSoftAim();
            //m_TargetPitch = (m_PitchDelta - m_Blackboard.m_PitchToRotateLeft) /
            //    (m_Blackboard.m_PitchToRotateRight - m_Blackboard.m_PitchToRotateLeft) * (1 + 1) - 1;
            //m_Blackboard.m_Animator.SetFloat("Pitch", m_TargetPitch);

            m_TargetForward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
            m_TargetForward.y = 0;
            transform.forward = m_TargetForward;
            m_Blackboard.m_Animator.SetBool("Moving", true);
            m_TargetVelocity = m_Blackboard.m_RunVelocity;
        });
        m_FSM.SetOnEnter(PlayerStates.RUN, () =>
        {
            ResetOnEnterTimers();

            m_Blackboard.m_Animator.SetBool("OnWall", false);

            m_PreviousRunInput = Vector2.zero;
            m_TargetAimWeight = 0;
            m_Blackboard.m_Animator.SetBool("Moving", true);
            m_TargetVelocity = m_Blackboard.m_RunVelocity;
        });
        m_FSM.SetOnEnter(PlayerStates.DASHING, () =>
        {
            ResetOnEnterTimers();

            m_Blackboard.m_Animator.SetBool("OnWall", false);

            if (!m_Input.Moving)
            {
                m_Controller.SetDashDirection(GameManager.GetManager().GetCameraManager().m_Camera);
                m_Blackboard.m_Animator.SetFloat("SpeedX", 0);
                m_Blackboard.m_Animator.SetFloat("SpeedY", 1);
                Vector3 l_lookDir = m_Controller.m_DashDirection.normalized;
                l_lookDir.y = 0;
                transform.rotation = Quaternion.LookRotation(l_lookDir);
            }
            if (m_SoftAimTimer < m_Blackboard.m_SoftAimTime || m_Input.Aiming)
            {
                m_Controller.SetDashDirection(GameManager.GetManager().GetCameraManager().m_Camera, m_Input.MovementAxis);
                m_Blackboard.m_Animator.SetFloat("SpeedX", m_TargetAnimatorSpeedX);
                m_Blackboard.m_Animator.SetFloat("SpeedY", m_TargetAnimatorSpeedY);
            }
            else
            {
                m_Controller.SetDashDirection(GameManager.GetManager().GetCameraManager().m_Camera, m_Input.MovementAxis);
                m_Blackboard.m_Animator.SetFloat("SpeedX", 0);
                m_Blackboard.m_Animator.SetFloat("SpeedY", 1);
                Vector3 l_lookDir = m_Controller.m_DashDirection.normalized;
                l_lookDir.y = 0;
                transform.rotation = Quaternion.LookRotation(l_lookDir);
            }

            StartCoroutine(SetAimWeight(m_Blackboard.m_AimRig, 0));
            m_Blackboard.m_Animator.SetBool("Dash", true);
            //OnStartDashing?.Invoke();
            m_Input.Aiming = false;
            GameManager.GetManager().GetCanvasManager().HideReticle();
            m_CurrentSpeed = m_Blackboard.m_DashVelocity;
            m_DashColdownTimer = 0.0f;
            m_DashTimer = 0.0f;
            m_Blackboard.m_DashTrail.SetActive(true);

            m_Dashing = true;
        });
        m_FSM.SetOnEnter(PlayerStates.FALL, () =>
        {
            ResetOnEnterTimers();

            m_Blackboard.m_Animator.SetBool("OnWall", false);

            m_Blackboard.m_Animator.SetBool("Ground", false);
            m_FallTimer = 0;
            m_LandTimer = 0;
            m_Land = false;
        });
        #endregion

        #region STAY
        //UPDATE
        m_FSM.SetOnStay(PlayerStates.INITIAL, () =>
        {
            m_FSM.ChangeState(PlayerStates.IDLE);
        });
        m_FSM.SetOnStay(PlayerStates.IDLE, () =>
        {
            WeightUpdate();

            m_Controller.GravityUpdate();
            m_Controller.MovementUpdate(m_Input.MovementAxis, GameManager.GetManager().GetCameraManager().m_Camera);

            MoveSpeedUpdate();

            //DeltaPitchUpdate();
            //DeltaYawUpdate();

            //LegRotationUpdate();

            //OnWallUpdate();

            TransitionConditions();
        });
        m_FSM.SetOnStay(PlayerStates.AIM, () =>
        {
            WeightUpdate();

            m_Controller.GravityUpdate();
            m_Controller.MovementUpdate(m_Input.MovementAxis, GameManager.GetManager().GetCameraManager().m_Camera);

            MoveSpeedUpdate();

            //DeltaPitchUpdate();
            DeltaYawUpdate();

            //LegRotationUpdate();

            m_TargetPitch = 0;

            ForwardUpdate();

            YawRotationUpdate();

            OnWallUpdate();

            TransitionConditions();
        });
        m_FSM.SetOnStay(PlayerStates.SOFTAIM, () =>
        {
            WeightUpdate();

            m_Controller.GravityUpdate();
            m_Controller.MovementUpdate(m_Input.MovementAxis, GameManager.GetManager().GetCameraManager().m_Camera);

            MoveSpeedUpdate();

            DeltaPitchUpdateSoftAim();
            DeltaYawUpdate();

            //PitchRotationUpdate();
            YawRotationUpdate();

            ForwardUpdate();

            OnWallUpdate();

            TransitionConditions();
        });
        m_FSM.SetOnStay(PlayerStates.AIM_WALK, () =>
        {
            WeightUpdate();

            SpeedUpdate();

            m_Controller.SetMovement(m_CurrentSpeed);
            m_Controller.GravityUpdate();
            m_Controller.MovementUpdate(m_Input.MovementAxis, GameManager.GetManager().GetCameraManager().m_Camera);

            MoveSpeedUpdate();

            //DeltaPitchUpdate();
            DeltaYawUpdate();

            m_TargetPitch = 0;

            ForwardUpdate();

            YawRotationUpdate();

            OnWallUpdate();

            TransitionConditions();
        });
        m_FSM.SetOnStay(PlayerStates.SOFTAIM_RUN, () =>
        {
            WeightUpdate();

            SpeedUpdate();

            m_Controller.SetMovement(m_CurrentSpeed);
            m_Controller.GravityUpdate();
            m_Controller.MovementUpdate(m_Input.MovementAxis, GameManager.GetManager().GetCameraManager().m_Camera);

            MoveSpeedUpdate();

            DeltaPitchUpdateSoftAim();
            DeltaYawUpdate();

            //PitchRotationUpdate();
            YawRotationUpdate();

            ForwardUpdate();

            OnWallUpdate();

            TransitionConditions();
        });
        m_FSM.SetOnStay(PlayerStates.RUN, () =>
        {
            WeightUpdate();

            SpeedUpdate();

            m_Controller.MovementUpdate(m_Input.MovementAxis, GameManager.GetManager().GetCameraManager().m_Camera);

            if (m_Input.Moving)
            {
                LookAtUpdate();
            }

            m_Controller.GravityUpdate();
            m_Controller.SetMovement(m_CurrentSpeed);

            float l_MovePercentage = m_MoveTimer / m_Blackboard.m_MoveTime;
            m_Blackboard.m_Animator.SetFloat("Speed", Mathf.Lerp(m_Blackboard.m_Animator.GetFloat("Speed"), 1,
                m_Blackboard.m_AnimCurveMove.Evaluate(l_MovePercentage)));
            m_MoveTimer += Time.deltaTime;

            DeltaYawUpdate();

            m_TargetPitch = 0;

            //OnWallUpdate();

            TransitionConditions();
        });
        m_FSM.SetOnStay(PlayerStates.DASHING, () =>
        {
            if (m_DashTimer < m_Blackboard.m_DashTime)
            {
                m_Controller.GravityUpdate();
                m_Controller.SetMovement(m_CurrentSpeed);
            }
            else
            {
                TransitionConditions();
            }
            m_DashTimer += Time.deltaTime;
        });
        m_FSM.SetOnStay(PlayerStates.FALL, () =>
        {
            if (!m_Land)
            {
                m_Controller.JumpMovementUpdate(m_Input.MovementAxis, GameManager.GetManager().GetCameraManager().m_Camera);
                m_Controller.GravityUpdate();
                m_Controller.SetMovement(m_CurrentSpeed);

                if (m_Controller.OnGround())
                {
                    m_Blackboard.m_Animator.SetBool("Ground", true);
                    if (m_FallTimer >= m_Blackboard.m_TimeToLand)
                    {
                        m_Blackboard.m_Animator.SetTrigger("Land");
                        m_Land = true;
                    }
                    else
                    {
                        m_FSM.ChangeState(PlayerStates.IDLE);
                    }
                }

                ForwardUpdate();

                m_FallTimer += Time.deltaTime;
            }
            else
            {
                m_Controller.GravityUpdate();
                m_Controller.SetMovement(m_CurrentSpeed);
                if (m_LandTimer >= m_Blackboard.m_LandTime)
                {
                    m_FSM.ChangeState(PlayerStates.IDLE);
                }
                m_LandTimer += Time.deltaTime;
            }
        });
        #endregion

        #region EXIT
        m_FSM.SetOnExit(PlayerStates.DASHING, () =>
        {
            m_Blackboard.m_Animator.SetBool("OnWall", false);

            m_Blackboard.m_Animator.SetBool("Dash", false);
            m_Blackboard.m_DashTrail.SetActive(false);
            m_CurrentSpeed = m_Blackboard.m_RunVelocity;
            GameManager.GetManager().GetCanvasManager().ShowReticle();
            m_Controller.ResetDashDirection();
        });
        #endregion
    }
    #region Functions
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
        m_Blackboard.m_Animator.SetTrigger("Shoot");
        m_SoftAimTimer = 0;
    }
    private void LegRotationUpdate()
    {
        if (!m_Rotated)
        {
            m_TargetForward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
            m_TargetForward.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, m_TargetForward, m_RotateTimer / m_Blackboard.m_RotateTime);
            m_PitchDelta = Mathf.Lerp(m_PitchDelta, 0, m_RotateTimer / m_Blackboard.m_RotateTime);
            if (!m_Blackboard.m_RigController.m_Rotate)
            {
                m_Blackboard.m_Animator.ResetTrigger("TurnR");
                m_Blackboard.m_Animator.ResetTrigger("TurnL");
                m_AnimationRotating = false;
                transform.forward = m_TargetForward;
                m_PitchDelta = 0;
                m_RotateTimer = 0;
                m_Rotated = true;
            }
        }

        //RIGHT
        if (m_TargetPitch >= 0.9f)
        {
            if (!m_Blackboard.m_RigController.m_Rotate)
            {
                m_Blackboard.m_Animator.SetTrigger("TurnR");
                m_RotateTimer = 0;
                m_Rotated = false;
                m_Blackboard.m_RigController.m_Rotate = true;
            }
        }
        //LEFT
        else if (m_TargetPitch <= -0.9f)
        {
            if (!m_Blackboard.m_RigController.m_Rotate)
            {
                m_Blackboard.m_Animator.SetTrigger("TurnL");
                m_RotateTimer = 0;
                m_Rotated = false;
                m_Blackboard.m_RigController.m_Rotate = true;
            }
        }

        m_RotateTimer += Time.deltaTime;
    }
    private void PitchRotationUpdate()
    {
        m_TargetPitch = (m_PitchDelta - m_Blackboard.m_PitchToRotateLeft) /
            (m_Blackboard.m_PitchToRotateRight - m_Blackboard.m_PitchToRotateLeft) * (1 + 1) - 1;

        float l_AimPercentage = m_Blackboard.m_AimTime / m_AimTimer;
        m_Blackboard.m_Animator.SetFloat("Pitch", Mathf.Lerp(m_Blackboard.m_Animator.GetFloat("Pitch"),
            m_TargetPitch, m_Blackboard.m_AnimCurveAim.Evaluate(l_AimPercentage)));
        m_AimTimer += Time.deltaTime;
    }
    private void YawRotationUpdate()
    {
        m_TargetYaw = (m_YawDelta - m_Blackboard.m_MinYaw) / (m_Blackboard.m_MaxYaw - m_Blackboard.m_MinYaw) * (1 + 1) - 1;

        if (m_YawDelta == m_PreviousYawDelta)
        {
            m_AimTimer = 0;
            m_PreviousYawDelta = m_YawDelta;
        }

        float l_AimPercentage = m_AimTimer / m_Blackboard.m_AimTime;
        m_Blackboard.m_Animator.SetFloat("Yaw", Mathf.Lerp(m_Blackboard.m_Animator.GetFloat("Yaw"),
            m_TargetYaw, m_Blackboard.m_AnimCurveAim.Evaluate(l_AimPercentage)));
        m_AimTimer += Time.deltaTime;
    }
    private void MoveSpeedUpdate()
    {
        m_TargetAnimatorSpeedX = 0;
        m_TargetAnimatorSpeedY = 0;

        if (m_Input.MovementAxis.x > 0)
        {
            m_TargetAnimatorSpeedX = 1;
        }
        else if (m_Input.MovementAxis.x < 0)
        {
            m_TargetAnimatorSpeedX = -1;
        }
        if (m_Input.MovementAxis.y > 0)
        {
            m_TargetAnimatorSpeedY = 1;
        }
        else if (m_Input.MovementAxis.y < 0)
        {
            m_TargetAnimatorSpeedY = -1;
        }

        float l_MovePercentage = m_MoveTimer / m_Blackboard.m_MoveTime;
        m_Blackboard.m_Animator.SetFloat("SpeedX", Mathf.Lerp(m_Blackboard.m_Animator.GetFloat("SpeedX"), m_TargetAnimatorSpeedX,
            m_Blackboard.m_AnimCurveMove.Evaluate(l_MovePercentage)));
        m_Blackboard.m_Animator.SetFloat("SpeedY", Mathf.Lerp(m_Blackboard.m_Animator.GetFloat("SpeedY"), m_TargetAnimatorSpeedY,
            m_Blackboard.m_AnimCurveMove.Evaluate(l_MovePercentage)));
        m_MoveTimer += Time.deltaTime;
    }
    private void ForwardUpdate()
    {
        m_TargetForward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
        m_TargetForward.y = 0;
        float l_LookAtPercentage = m_LookAtTimer / m_Blackboard.m_LookAtTime;
        transform.forward = Vector3.Lerp(transform.forward, m_TargetForward, 
            m_Blackboard.m_AnimCurveSpeed.Evaluate(l_LookAtPercentage));
        m_LookAtTimer += Time.deltaTime;
    }
    private void DeltaPitchUpdate()
    {
        float l_CameraPitch = GameManager.GetManager().GetCameraManager().m_Camera.transform.localEulerAngles.y;
        if (MathF.Abs(l_CameraPitch - m_PreviousCameraPitch) >= 350)
        {
            if (m_PreviousCameraPitch > l_CameraPitch)
            {
                m_PreviousCameraPitch = 360 - m_PreviousCameraPitch;
            }
            else
            {
                m_PreviousCameraPitch = 360 + m_PreviousCameraPitch;
            }
        }
        m_PitchDelta += l_CameraPitch - m_PreviousCameraPitch;
        if (m_PitchDelta >= m_Blackboard.m_PitchToRotateRight)
        {
            m_PitchDelta = m_Blackboard.m_PitchToRotateRight;
        }
        else if (m_PitchDelta <= m_Blackboard.m_PitchToRotateLeft)
        {
            m_PitchDelta = m_Blackboard.m_PitchToRotateLeft;
        }
        m_PreviousCameraPitch = l_CameraPitch;
    }
    private void DeltaPitchUpdateSoftAim()
    {
        float l_MovementRotation = 0;

        if (m_Input.MovementAxis.x > 0 && m_Input.MovementAxis.y > 0 || m_Input.MovementAxis.x > 0 && m_Input.MovementAxis.y < 0 ||
            m_Input.MovementAxis.x < 0 && m_Input.MovementAxis.y < 0 || m_Input.MovementAxis.x < 0 && m_Input.MovementAxis.y > 0)
        {
            l_MovementRotation = 45;
        }
        //else if ()
        //{
        //    l_MovementRotation = 45;
        //}

        float l_CameraPitch = GameManager.GetManager().GetCameraManager().m_Camera.transform.localEulerAngles.y + l_MovementRotation;
        float l_CharacterPitch = transform.rotation.eulerAngles.y;

        m_PitchDelta = l_CameraPitch - l_CharacterPitch;
    }
    private void DeltaYawUpdate()
    {
        m_YawDelta = -GameManager.GetManager().GetCameraManager().m_Camera.transform.localEulerAngles.x;
        if (m_YawDelta < -180)
        {
            m_YawDelta += 360;
        }
    }
    void Hit(float a)
    {
        int hit = UnityEngine.Random.Range(0, 3);
        m_Blackboard.m_Animator.SetInteger("Hit", hit);
    }
    void Death(GameObject a)
    {
        m_Blackboard.m_Animator.SetTrigger("Die");
    }
    void OnWallUpdate()
    {
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
    }
    private IEnumerator SetAimWeight(Rig rig, float weight)
    {
        yield return new WaitForEndOfFrame();
        rig.weight = weight;
        yield return null;
    }
    private void SpeedUpdate()
    {
        float l_SpeedPercentage = m_SpeedTimer / m_Blackboard.m_SpeedTime;
        m_CurrentSpeed = Mathf.Lerp(m_CurrentSpeed, m_TargetVelocity,
            m_Blackboard.m_AnimCurveSpeed.Evaluate(l_SpeedPercentage));
        m_SpeedTimer += Time.deltaTime;
    }
    private void LookAtUpdate()
    {
        float l_LookAtPercentage = m_LookAtTimer / m_Blackboard.m_LookAtTime;
        Vector3 l_lookDir = m_Controller.m_Direction.normalized;
        l_lookDir.y = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(l_lookDir), 
            m_Blackboard.m_AnimCurveSpeed.Evaluate(l_LookAtPercentage));
        m_LookAtTimer += Time.deltaTime;
    }
    private void WeightUpdate()
    {
        float l_WeightPercentage = m_WeightTimer / m_Blackboard.m_WeightTime;
        StartCoroutine(SetAimWeight(m_Blackboard.m_AimRig,
            Mathf.Lerp(m_Blackboard.m_AimRig.weight, m_TargetAimWeight,
            m_Blackboard.m_AnimCurveWeight.Evaluate(l_WeightPercentage))));
        m_WeightTimer += Time.deltaTime;
    }
    private void ResetOnEnterTimers()
    {
        m_SpeedTimer = 0;
        m_MoveTimer = 0;
        m_LookAtTimer = 0;
        m_WeightTimer = 0;
}
    private void ResetOnChangeDir()
    {
        m_MoveTimer = 0;
        m_LookAtTimer = 0;
    }
    #endregion
    private void TransitionConditions()
    {
        if (!m_Controller.OnGround())
        {
            if (m_FSM.currentState == PlayerStates.FALL) { return; }
            m_FSM.ChangeState(PlayerStates.FALL);
            return;
        }
        if (m_Input.Dashing && !m_Input.Aiming)
        {
            if (m_DashColdownTimer >= m_Blackboard.m_DashColdownTime)
            {
                if (m_FSM.currentState == PlayerStates.DASHING) { return; }
                m_FSM.ChangeState(PlayerStates.DASHING);
                return;
            }
        }
        if (!m_Input.Moving)
        {
            if (m_Input.Aiming)
            {
                if (m_FSM.currentState == PlayerStates.AIM) { return; }
                m_FSM.ChangeState(PlayerStates.AIM);
                return;
            }
            if (m_SoftAimTimer < m_Blackboard.m_SoftAimTime)
            {
                if (m_FSM.currentState == PlayerStates.SOFTAIM) { return; }
                m_FSM.ChangeState(PlayerStates.SOFTAIM);
                return;
            }
            else if (!m_Input.Aiming)
            {
                if (m_FSM.currentState == PlayerStates.IDLE) { return; }
                m_FSM.ChangeState(PlayerStates.IDLE);
                return;
            }
        }
        else
        {
            if (m_Input.Aiming)
            {
                if (m_FSM.currentState == PlayerStates.AIM_WALK) { return; }
                m_FSM.ChangeState(PlayerStates.AIM_WALK);
                return;
            }
            else if (m_SoftAimTimer < m_Blackboard.m_SoftAimTime)
            {
                if (m_FSM.currentState == PlayerStates.SOFTAIM_RUN) { return; }
                m_FSM.ChangeState(PlayerStates.SOFTAIM_RUN);
                return;
            }
            else
            {
                if (m_FSM.currentState == PlayerStates.RUN) { return; }
                m_FSM.ChangeState(PlayerStates.RUN);
                return;
            }
        }
    }
}
