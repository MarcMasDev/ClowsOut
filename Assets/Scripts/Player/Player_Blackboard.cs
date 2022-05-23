using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Blackboard : MonoBehaviour
{
    [Header("Animator")]
    public Animator m_Animator;
    public float m_LerpAnimationMovementPct;
    public GameObject m_Center;
    public GameObject m_Feet;
    public LayerMask m_GroundLayerMask;
    [Header("Movement")]
    public float m_LerpRotationPct = 0.1f;
    public float m_WalkVelocity = 3;
    public float m_AimVelocity;
    public float m_DashVelocity;
    public GameObject m_DashTrail;
    public float m_DashTime;
    public float m_DashColdownTime;
    public float m_PitchToRotateRight;
    public float m_PitchToRotateLeft;
    public float m_SlopeForce;
    public float m_TimeToLand;
    public GameObject m_AimTarget;
    [Header("Shoot")]
    [Range(0, 5.0f)] public float m_RateOfFire;
    public Transform m_ShootPoint;
    public LayerMask m_AimLayers;
    public float m_AimMaxDistance;
    public float m_ReloadTime;
    public float m_ShootTime;
    public float m_BulletSpeed;
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
