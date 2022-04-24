using UnityEngine;

public class Player_Blackboard : MonoBehaviour
{
    [Header("Basic")]
    public Camera m_Camera;
    [Header("Movement")]
    public float m_LerpRotationPct = 0.1f;
    public float m_WalkVelocity = 3;
    public float m_AimVelocity;
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
}
