using System;
using UnityEngine;

[RequireComponent(typeof(Player_InputHandle))]
public class Player_ShootSystem : MonoBehaviour
{
    public LineRenderer m_LineRendererAim;
    public LineRenderer m_LineRendererShoot;
    [Header("Ammunition Capacity")]
    [Range(0, 8)] public int m_AmmunitionCapacity;
    [Header("Rate Of Fire")]
    [Range(0, 5.0f)] public float m_RateOfFire;
    //TODO: Implement recoil or only shake camera
    //[Header("Recoil")]
    //[Range(0, 15.0f)] public float m_VerticalMaximumRecoil = 0.7f;
    //[Range(0, 15.0f)] public float m_VerticalMinimumRecoil = 0.3f;
    //[Range(0, 3.0f)] public float m_HorizontalMaximumRecoil = 0.45f;
    //[Range(0, -3.0f)] public float m_HorizontalMinimumRecoil = -0.45f;
    [Header("Aiming")]
    public Camera m_Camera;
    public Transform m_ShootPoint;
    public LayerMask m_AimLayers;
    public float m_AimMaxDistance;
    [Header("Animations Time")]
    public float m_ReloadTime;
    public float m_ShootTime;
    //TODO: Animation time

    public event Action OnShoot;

    private int m_ContinuousBulletsFired;
    private float m_RateOfFireTimer;
    private float m_ReloadTimer;
    private float m_MagazineAmmunition;
    private float m_ShootTimer;
    private Vector3 m_AimPoint;
    private float m_CurrentDispersion;

    private Player_InputHandle m_Input;
    private Dispersion m_Dispersion;

    void Awake()
    {
        m_Input = GetComponent<Player_InputHandle>();
        m_Dispersion = GetComponent<Dispersion>();
        m_MagazineAmmunition = m_AmmunitionCapacity;
        m_RateOfFireTimer = m_RateOfFire;
        m_ReloadTimer = m_ReloadTime;
        m_ShootTimer = m_ShootTime;
    }

    void Update()
    {
        if (CanShoot())
        {
            Debug.Log("Shoot");
            Shoot();
        }
        else
        {
            m_ContinuousBulletsFired = 0;
        }
        m_RateOfFireTimer += Time.deltaTime;
        m_ShootTimer += Time.deltaTime;

        if (CanAutomaticReload())
        {
            Reload();
        }
        m_ReloadTimer += Time.deltaTime;

        /*
         * TODO:
        if (m_ShootTimer > m_ShootTime && m_ReloadTimer > m_ReloadTime)
        {
        maybe idle aiming animation
        }
        */
    }
    private bool CanShoot()
    {
        return m_Input.Shooting && m_Input.Aiming && m_RateOfFireTimer >= m_RateOfFire && m_ReloadTimer >= m_ReloadTime && m_MagazineAmmunition > 0;
    }
    private void Shoot()
    {
        m_CurrentDispersion = m_Dispersion.m_CurrentDispersion;
        m_CurrentDispersion *= Mathf.Deg2Rad;

        RaycastHit l_Hit;
        if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out l_Hit, m_AimMaxDistance, m_AimLayers))
        {
            m_AimPoint = l_Hit.point;
        }
        else
        {
            m_AimPoint = m_ShootPoint.transform.position + m_Camera.transform.forward * m_AimMaxDistance;
        }
        m_LineRendererAim.positionCount = 2;
        m_LineRendererAim.SetPosition(0, m_ShootPoint.transform.position);
        m_LineRendererAim.SetPosition(1, m_AimPoint);

        CreateBullet();

        OnShoot?.Invoke();
        //TODO: fireFX / Sound / Animation / Change Hud (ammo)
        m_ContinuousBulletsFired += 1;
        m_MagazineAmmunition -= 1;
        m_RateOfFireTimer = 0;
        m_ShootTimer = 0;
    }
    private void CreateBullet()
    {
        //float yawInRadians = _yaw * Mathf.Deg2Rad;
        //float pitchInRadians = -_pitch * Mathf.Deg2Rad;
        //AimNormal = new Vector3(Mathf.Sin(yawInRadians), Mathf.Sin(pitchInRadians), Mathf.Cos(yawInRadians));

        //TODO: Ainoa Shoot System
        Vector3 l_AimNormal = (m_AimPoint - m_ShootPoint.transform.position).normalized;
        Vector3 l_BulletNormal = (l_AimNormal + BulletDispersion()).normalized;

        RaycastHit l_Hit;
        Vector3 l_ShootPoint;
        if (Physics.Raycast(m_ShootPoint.transform.position, l_BulletNormal, out l_Hit, m_AimMaxDistance, m_AimLayers))
        {
            l_ShootPoint = l_Hit.point;
        }
        else
        {
            l_ShootPoint = m_ShootPoint.transform.position + l_BulletNormal * m_AimMaxDistance;
        }
        m_LineRendererShoot.positionCount = 2;
        m_LineRendererShoot.SetPosition(0, m_ShootPoint.transform.position);
        m_LineRendererShoot.SetPosition(1, l_ShootPoint);

        //m_LineRendererShoot
        //BulletManager.GetBulletManager().CreateBullet(_playerCamera.transform.position, normal, _bulletSpeed, _shootingLayerMask);
    }
    private Vector3 BulletDispersion()
    {
        Vector3 l_Dispersion = new Vector3(UnityEngine.Random.Range(-m_CurrentDispersion, m_CurrentDispersion), UnityEngine.Random.Range(-m_CurrentDispersion, m_CurrentDispersion), 0.0f);
        while (Mathf.Pow(l_Dispersion.x, 2) + Mathf.Pow(l_Dispersion.y, 2) > Mathf.Pow(m_CurrentDispersion, 2))
        {
            l_Dispersion = new Vector3(UnityEngine.Random.Range(-m_CurrentDispersion, m_CurrentDispersion), UnityEngine.Random.Range(-m_CurrentDispersion, m_CurrentDispersion), 0.0f);
        }
        return l_Dispersion;
    }
    private bool CanAutomaticReload()
    {
        return m_ShootTimer > m_ShootTime && m_MagazineAmmunition == 0 && m_ReloadTimer > m_ReloadTime;
    }
    private void Reload()
    {
        m_MagazineAmmunition = m_AmmunitionCapacity;
        //TODO: Sound / Animation / Change Hud (ammo)
        m_ReloadTimer = 0;
    }
}
