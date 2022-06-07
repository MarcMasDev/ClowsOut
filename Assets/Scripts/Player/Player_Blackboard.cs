using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;

public class Player_Blackboard : MonoBehaviour
{
    [Header("Components")]
    public Animator m_Animator;
    public GameObject m_AimTarget;
    public GameObject m_Center;
    public GameObject m_CenterW;
    public GameObject m_DashTrail;
    public GameObject m_Feet;
    public FMODDolores m_FmodDolores;
    public GameObject m_Hand;
    public Rig m_AimRig;
    public RigController m_RigController;
    public Transform m_EnemyAimPoint;
    public Transform m_ShootPoint;
    [Header("Transitions")]
    public AnimationCurve m_AnimCurveSpeed;
    public float m_SpeedTime;
    public AnimationCurve m_AnimCurveMove;
    public float m_MoveTime;
    public AnimationCurve m_AnimCurveLookAt;
    public float m_LookAtTime;
    public AnimationCurve m_AnimCurveWeight;
    public float m_WeightTime;
    public AnimationCurve m_AnimCurveAim;
    public float m_AimTime;
    public float m_SoftAimTime;
    [Header("Movement")]
    public float m_DashVelocity;
    public float m_RunVelocity = 3;
    public float m_WalkVelocity;
    public float m_AirSpeed;
    public float m_DashColdownTime;
    public float m_DashTime;
    public float m_MaxYaw;
    public float m_MinYaw;
    public float m_PitchToRotateLeft;
    public float m_PitchToRotateRight;
    public float m_RotateTime;
    public float m_SlopeForce;
    public float m_TimeToLand;
    public bool m_Teleported = true;
    public LayerMask m_GroundLayerMask;
    [Header("Shoot")]
    [Range(0, 5.0f)] public float m_RateOfFire;
    public bool m_OnWall;
    public float m_AimMaxDistance;
    public float m_BulletSpeed;
    public float m_ReloadTime;
    public float m_ShootTime;
    public LayerMask m_AimLayers,m_CollisionWithEffect;
    [Header("Dispersion")]
    [Range(0, 4.0f)] public float m_ShootDispersion;
    [Range(0, 4.0f)] public float m_DefaultDispersion;
    [Range(0, 4.0f)] public float m_AimDispersion;
    [Range(0, 4.0f)] public float m_MovementAddDispersion;
    [Range(0, 20.0f)] public float m_DefaultSpeed;
    [Range(0, 20.0f)] public float m_AimSpeed;
    [Range(0, 20.0f)] public float m_ShootSpeed;
    [Range(0, 20.0f)] public float m_RecoverSpeed;
    [Header("Interact")]
    public float m_InteractDistance;
    public LayerMask m_InteractLayers;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += Init;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Init;
    }
    private void Init(Scene scene, LoadSceneMode mode)
    {
        GameManager.GetManager().SetPlayer(gameObject);
    }
}
