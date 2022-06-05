using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FSM : MonoBehaviour, IRestart
{
    private enum PlayerStates { INITIAL, IDLE, MOVE, MOVE_LOCK, FALL, DASHING, DYING }
    #region Variables
    private FSM<PlayerStates> m_FSM;
    private Vector3 m_TargetForward;
    private Vector3 m_InitalPos;
    private float m_YawDelta;
    private float m_TargetVelocity;
    private float m_TargetTorsoYaw;
    private float m_TargetTorsoPitch;
    private float m_TargetAnimatorSpeedY;
    private float m_TargetAnimatorSpeedX;
    private float m_SoftAimTimer;
    private float m_RotateTimer;
    private float m_PreviousCameraPitch;
    private float m_PitchDelta;
    private float m_FallTimer;
    private float m_DashTimer;
    private float m_DashColdownTimer;
    private float m_CurretVelocity;
    private bool m_Rotated;
    private bool m_AnimationRotating;
    private Vector2 m_PreviousMoveInput;
    #endregion
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
        m_FSM.Update();
        if (m_Input.Aiming)
        {
            m_Blackboard.m_Animator.SetBool("Aim", true);
        }
        else
        {
            m_Blackboard.m_Animator.SetBool("Aim", false);
        }
        if (m_Input.Aiming)
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
        Debug.Log(m_FSM.currentState);
        //Stop FSM When dying
    }
    private void InitFSM()
    {
        //START
        m_FSM = new FSM<PlayerStates>(PlayerStates.INITIAL);
        m_FSM.SetReEnter(() =>
        {
            m_FSM.ChangeState(PlayerStates.INITIAL);
        });
        //ENTER
        m_FSM.SetOnEnter(PlayerStates.IDLE, () =>
        {
            StartCoroutine(SetAimWeight());
            m_TargetVelocity = 0;
            m_Blackboard.m_Animator.SetBool("Moving", false);
        });
        m_FSM.SetOnEnter(PlayerStates.MOVE_LOCK, () =>
        {
            m_Blackboard.m_Animator.SetBool("Moving", true);
            m_Blackboard.m_Animator.SetFloat("Speed", 0);
            m_TargetVelocity = m_Blackboard.m_RunVelocity;
        });
        m_FSM.SetOnEnter(PlayerStates.MOVE, () =>
        {
            m_Blackboard.m_Animator.SetBool("Moving", true);
        });
        m_FSM.SetOnEnter(PlayerStates.DASHING, () =>
        {
            m_Blackboard.m_Animator.SetBool("Dash", true);
            //OnStartDashing?.Invoke();
            m_Input.Aiming = false;
            GameManager.GetManager().GetCanvasManager().HideReticle();
            m_CurretVelocity = m_Blackboard.m_DashVelocity;
            m_DashColdownTimer = 0.0f;
            m_DashTimer = 0.0f;
            m_Blackboard.m_DashTrail.SetActive(true);
        });
        m_FSM.SetOnEnter(PlayerStates.FALL, () =>
        {
            m_Blackboard.m_Animator.SetBool("Ground", false);
        });
        //UPDATE
        m_FSM.SetOnStay(PlayerStates.INITIAL, () =>
        {
            m_FSM.ChangeState(PlayerStates.IDLE);
        });
        m_FSM.SetOnStay(PlayerStates.MOVE_LOCK, () =>
        {
            m_CurretVelocity = Mathf.Lerp(m_CurretVelocity, m_TargetVelocity, m_Blackboard.m_LerpAnimationMovementPct);
            m_Controller.SetMovement(m_CurretVelocity);
            m_Controller.GravityUpdate();
            m_Controller.MovementUpdate(m_Input.MovementAxis, GameManager.GetManager().GetCameraManager().m_Camera);

            m_Blackboard.m_Animator.SetFloat("Speed", Mathf.Lerp(m_Blackboard.m_Animator.GetFloat("Speed"), 1,
                m_Blackboard.m_LerpAnimationMovementPct));

            Vector3 l_lookDir = m_Controller.m_Direction.normalized;
            l_lookDir.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(l_lookDir), m_Blackboard.m_LerpRotationPct);

            OnWallUpdate();

            if (!m_Input.Moving)
            {
                m_FSM.ChangeState(PlayerStates.IDLE);
            }
            if (m_SoftAimTimer <= m_Blackboard.m_SoftAimTime || m_Input.Aiming)
            {
                m_FSM.ChangeState(PlayerStates.MOVE);
            }
            if (m_Input.Dashing)
            {
                if (m_DashColdownTimer >= m_Blackboard.m_DashColdownTime)
                {
                    m_Controller.SetDashDirection(GameManager.GetManager().GetCameraManager().m_Camera, m_Input.MovementAxis);
                    m_Blackboard.m_Animator.SetFloat("LegsX", m_TargetAnimatorSpeedX);
                    m_Blackboard.m_Animator.SetFloat("LegsZ", m_TargetAnimatorSpeedY);
                    m_FSM.ChangeState(PlayerStates.DASHING);
                }
            }
            else if (!m_Controller.OnGround())
            {
                m_FSM.ChangeState(PlayerStates.FALL);
            }
        });
        m_FSM.SetOnStay(PlayerStates.MOVE, () =>
        {
            if (!m_Input.Aiming)
            {
                m_TargetVelocity = m_Blackboard.m_RunVelocity;
                //m_Blackboard.m_AimRig.weight = 0;
            }
            else
            {
                m_TargetVelocity = m_Blackboard.m_WalkVelocity;
                //m_Blackboard.m_AimRig.weight = 1;
            }

            m_CurretVelocity = Mathf.Lerp(m_CurretVelocity, m_TargetVelocity, m_Blackboard.m_LerpAnimationMovementPct);
            m_Controller.SetMovement(m_CurretVelocity);
            m_Controller.GravityUpdate();
            m_Controller.MovementUpdate(m_Input.MovementAxis, GameManager.GetManager().GetCameraManager().m_Camera);

            MoveSpeedUpdate();

            DeltaPitchUpdate();
            DeltaYawUpdate();

            m_TargetTorsoPitch = 0;

            MoveBodyRotationUpdate();

            //TorsoRotationUpdate();

            OnWallUpdate();

            if (!m_Input.Moving)
            {
                m_FSM.ChangeState(PlayerStates.IDLE);
            }
            if (!(m_SoftAimTimer <= m_Blackboard.m_SoftAimTime || m_Input.Aiming))
            {
                m_FSM.ChangeState(PlayerStates.MOVE_LOCK);
            }
            if (m_Input.Dashing)
            {
                if (m_DashColdownTimer >= m_Blackboard.m_DashColdownTime)
                {
                    m_Controller.SetDashDirection(GameManager.GetManager().GetCameraManager().m_Camera, m_Input.MovementAxis);
                    m_Blackboard.m_Animator.SetFloat("LegsX", m_TargetAnimatorSpeedX);
                    m_Blackboard.m_Animator.SetFloat("LegsZ", m_TargetAnimatorSpeedY);
                    m_FSM.ChangeState(PlayerStates.DASHING);
                }
            }
            else if (!m_Controller.OnGround())
            {
                m_FSM.ChangeState(PlayerStates.FALL);
            }
        });
        m_FSM.SetOnStay(PlayerStates.IDLE, () =>
        {
            m_CurretVelocity = Mathf.Lerp(m_CurretVelocity, m_TargetVelocity, m_Blackboard.m_LerpAnimationMovementPct);
            m_Controller.SetMovement(m_CurretVelocity);
            m_Controller.GravityUpdate();
            m_Controller.MovementUpdate(m_Input.MovementAxis, GameManager.GetManager().GetCameraManager().m_Camera);

            MoveSpeedUpdate();

            DeltaPitchUpdate();
            DeltaYawUpdate();

            //LegRotationUpdate();

            TorsoRotationUpdate();

            OnWallUpdate();

            if (m_Input.Moving)
            {
                if (m_SoftAimTimer >= m_Blackboard.m_SoftAimTime)
                {
                    m_FSM.ChangeState(PlayerStates.MOVE_LOCK);
                }
                else
                {
                    m_FSM.ChangeState(PlayerStates.MOVE);
                }
            }
            if (m_Input.Dashing)
            {
                if (m_DashColdownTimer >= m_Blackboard.m_DashColdownTime)
                {
                    m_Controller.SetDashDirection(GameManager.GetManager().GetCameraManager().m_Camera);
                    m_Blackboard.m_Animator.SetFloat("SpeedX", 0);
                    m_Blackboard.m_Animator.SetFloat("SpeedZ", 1);
                    m_FSM.ChangeState(PlayerStates.DASHING);
                }
            }
            else if (!m_Controller.OnGround())
            {
                m_FSM.ChangeState(PlayerStates.FALL);
            }
        });

        m_FSM.SetOnStay(PlayerStates.DASHING, () =>
        {
            if (m_DashTimer < m_Blackboard.m_DashTime)
            {
                m_Controller.GravityUpdate();
                m_Controller.SetMovement(m_CurretVelocity);
            }
            else
            {
                m_FSM.ChangeState(PlayerStates.MOVE);
                m_DashColdownTimer = 0;
            }
            m_DashTimer += Time.deltaTime;
        });

        m_FSM.SetOnStay(PlayerStates.FALL, () =>
        {
            m_Controller.JumpMovementUpdate(m_Input.MovementAxis, GameManager.GetManager().GetCameraManager().m_Camera);
            m_Controller.GravityUpdate();
            m_Controller.SetMovement(m_CurretVelocity);

            if (m_Controller.OnGround())
            {
                m_FSM.ChangeState(PlayerStates.MOVE);
            }
            m_TargetForward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
            m_TargetForward.y = 0;

            transform.forward = Vector3.Lerp(transform.forward, m_TargetForward, m_Blackboard.m_LerpRotationPct * 4);

            m_FallTimer += Time.deltaTime;
        });

        //EXIT
        m_FSM.SetOnExit(PlayerStates.FALL, () =>
        {
            if (m_FallTimer >= m_Blackboard.m_TimeToLand)
            {
                m_Blackboard.m_Animator.SetTrigger("Land");
            }
            m_Blackboard.m_Animator.SetBool("Ground", true);
            m_FallTimer = 0;
        });
        m_FSM.SetOnExit(PlayerStates.DASHING, () =>
        {
            m_Blackboard.m_Animator.SetBool("Dash", false);
            m_Blackboard.m_DashTrail.SetActive(false);
            m_CurretVelocity = m_Blackboard.m_RunVelocity;
            GameManager.GetManager().GetCanvasManager().ShowReticle();
            m_Controller.ResetDashDirection();
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
    #region Functions
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
                m_Blackboard.m_Animator.ResetTrigger("RotateRight");
                m_Blackboard.m_Animator.ResetTrigger("RotateLeft");
                m_AnimationRotating = false;
                transform.forward = m_TargetForward;
                m_PitchDelta = 0;
                m_RotateTimer = 0;
                m_Rotated = true;
            }
        }

        //RIGHT
        if (m_TargetTorsoPitch >= 0.9f)
        {
            if (!m_Blackboard.m_RigController.m_Rotate)
            {
                m_Blackboard.m_Animator.SetTrigger("RotateRight");
                m_RotateTimer = 0;
                m_Rotated = false;
                m_Blackboard.m_RigController.m_Rotate = true;
            }
        }
        //LEFT
        else if (m_TargetTorsoPitch <= -0.9f)
        {
            if (!m_Blackboard.m_RigController.m_Rotate)
            {
                m_Blackboard.m_Animator.SetTrigger("RotateLeft");
                m_RotateTimer = 0;
                m_Rotated = false;
                m_Blackboard.m_RigController.m_Rotate = true;
            }
        }

        m_RotateTimer += Time.deltaTime;
    }
    private void TorsoRotationUpdate()
    {
        m_TargetTorsoYaw = (m_YawDelta - m_Blackboard.m_MinYaw) / (m_Blackboard.m_MaxYaw - m_Blackboard.m_MinYaw) * (1 + 1) - 1;
        m_TargetTorsoPitch = (m_PitchDelta - m_Blackboard.m_PitchToRotateLeft) /
            (m_Blackboard.m_PitchToRotateRight - m_Blackboard.m_PitchToRotateLeft) * (1 + 1) - 1;

        m_Blackboard.m_Animator.SetFloat("TorsoYaw", Mathf.Lerp(m_Blackboard.m_Animator.GetFloat("TorsoYaw"),
            m_TargetTorsoYaw, m_Blackboard.m_LerpAnimationAimPct));

        m_Blackboard.m_Animator.SetFloat("TorsoPitch", Mathf.Lerp(m_Blackboard.m_Animator.GetFloat("TorsoPitch"),
            m_TargetTorsoPitch, m_Blackboard.m_LerpAnimationAimPct));
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
        m_Blackboard.m_Animator.SetFloat("SpeedX", Mathf.Lerp(m_Blackboard.m_Animator.GetFloat("SpeedX"), m_TargetAnimatorSpeedX,
            m_Blackboard.m_LerpAnimationMovementPct));
        m_Blackboard.m_Animator.SetFloat("SpeedY", Mathf.Lerp(m_Blackboard.m_Animator.GetFloat("SpeedY"), m_TargetAnimatorSpeedY,
            m_Blackboard.m_LerpAnimationMovementPct));
    }
    private void MoveBodyRotationUpdate()
    {
        m_TargetForward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
        m_TargetForward.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, m_TargetForward, m_Blackboard.m_LerpAnimationMovementPct);
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
    private IEnumerator SetAimWeight()
    {
        yield return new WaitForEndOfFrame();
        m_Blackboard.m_AimRig.weight = 1;
        yield return new WaitForSeconds(1f);
        yield return new WaitForEndOfFrame();
        m_Blackboard.m_AimRig.weight = 0;
        yield return new WaitForSeconds(1f);
        yield return SetAimWeight();
    }
    #endregion
}
