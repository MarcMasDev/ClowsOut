using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;

public class Player_Blackboard : MonoBehaviour
{
    [Header("Animator")]
    public Animator m_Animator;
    public float m_LerpAnimationAimPct;
    public float m_LerpAnimationMovementPct;
    public float m_LerpAnimationVelocityPct;
    public float m_StopAimTime;
    public GameObject m_Center;
    public GameObject m_CenterW;
    public GameObject m_Feet;
    public GameObject m_Hand;
    public LayerMask m_GroundLayerMask;
    public RigController m_RigController;
    [Header("Movement")]
    public float m_AimVelocity;
    public float m_AirSpeed;
    public float m_DashColdownTime;
    public float m_DashTime;
    public float m_DashVelocity;
    public float m_LerpRotationPct = 0.1f;
    public float m_MaxYaw;
    public float m_MinYaw;
    public float m_MoveVelocity = 3;
    public float m_PitchToRotateLeft;
    public float m_PitchToRotateRight;
    public float m_RotateTime;
    public float m_SlopeForce;
    public float m_TimeToLand;
    public GameObject m_AimTarget;
    public GameObject m_DashTrail;
    [Header("Shoot")]
    [Range(0, 5.0f)] public float m_RateOfFire;
    public bool m_OnWall;
    public float m_AimMaxDistance;
    public float m_BulletSpeed;
    public float m_ReloadTime;
    public float m_ShootTime;
    public bool m_CanShoot = true;
    public LayerMask m_AimLayers,m_CollisionWithEffect;
    public Transform m_ShootPoint;
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
