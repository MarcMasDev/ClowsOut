using System;
using UnityEngine;

[RequireComponent(typeof(Player_InputHandle))]
public class Player_ShootSystem : ShootSystem
{
    //TODO: Implement recoil or only shake camera
    //[Header("Recoil")]
    //[Range(0, 15.0f)] public float m_VerticalMaximumRecoil = 0.7f;
    //[Range(0, 15.0f)] public float m_VerticalMinimumRecoil = 0.3f;
    //[Range(0, 3.0f)] public float m_HorizontalMaximumRecoil = 0.45f;
    //[Range(0, -3.0f)] public float m_HorizontalMinimumRecoil = -0.45f;

    //TODO: Animation time

    public event Action OnShoot;

    [HideInInspector] public int m_ContinuousBulletsFired;
    private float m_RateOfFireTimer;
    private float m_ReloadTimer;
    private float m_ShootTimer;
    private Vector3 m_AimPoint;
    private float m_CurrentDispersion;

    private Player_InputHandle m_Input;
    private Player_Dispersion m_Dispersion;
    private Player_BulletManager m_BulletManager;
    private Player_Blackboard m_Blackboard;

    void Awake()
    {
        m_Input = GetComponent<Player_InputHandle>();
        m_Dispersion = GetComponent<Player_Dispersion>();
        m_BulletManager = GetComponent<Player_BulletManager>();
        m_Blackboard = GetComponent<Player_Blackboard>();
        m_RateOfFireTimer = m_Blackboard.m_RateOfFire;
        m_ReloadTimer = m_Blackboard.m_ReloadTime;
        m_ShootTimer = m_Blackboard.m_ShootTime;
    }

    void Update()
    {
        if (CanShoot())
        {
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
            //TODO: Sound / Animation / Change Hud (ammo)
            Reload();
        }
        m_ReloadTimer += Time.deltaTime;

        //updating bullets of shootsystem 
        base.UpdateShootSystem();
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
        return m_Input.Shooting && m_RateOfFireTimer >= 
            m_Blackboard.m_RateOfFire && m_ReloadTimer >= m_Blackboard.m_ReloadTime && !m_BulletManager.m_NoBullets;
    }
    private void Shoot()
    {
        m_CurrentDispersion = m_Dispersion.m_CurrentDispersion;
        m_CurrentDispersion *= Mathf.Deg2Rad;

        CreateBullet();

        OnShoot?.Invoke();
        //TODO: fireFX / Sound / Animation / Change Hud (ammo)
        m_ContinuousBulletsFired += 1;
        m_RateOfFireTimer = 0;
        m_ShootTimer = 0;
    }
    private void CreateBullet()
    {
        //TODO: Ainoa Shoot System
        Vector3 l_AimNormal = (m_AimPoint - m_Blackboard.m_ShootPoint.transform.position).normalized;
        Vector3 l_BulletNormal = (l_AimNormal + BulletDispersion()).normalized;

        //temporal type bullet var
        BulletShoot(m_Blackboard.m_ShootPoint.position, l_BulletNormal, m_BulletSpeed, m_BulletManager.m_CurrentBullet);
        m_BulletManager.NextBullet();
        //BulletManager.GetBulletManager().CreateBullet(_playerCamera.transform.position, normal, _bulletSpeed, _shootingLayerMask);
    }
    private Vector3 BulletDispersion()
    {
        Vector3 l_Dispersion = new Vector3(UnityEngine.Random.Range(-m_CurrentDispersion, m_CurrentDispersion), 
            UnityEngine.Random.Range(-m_CurrentDispersion, m_CurrentDispersion), 0.0f);

        while (Mathf.Pow(l_Dispersion.x, 2) + Mathf.Pow(l_Dispersion.y, 2) > Mathf.Pow(m_CurrentDispersion, 2))
        {
            l_Dispersion = new Vector3(UnityEngine.Random.Range(-m_CurrentDispersion, m_CurrentDispersion), 
                UnityEngine.Random.Range(-m_CurrentDispersion, m_CurrentDispersion), 0.0f);
        }
        return l_Dispersion;
    }
    private bool CanAutomaticReload()
    {
        return m_ShootTimer > m_Blackboard.m_ShootTime && m_BulletManager.m_NoBullets && m_ReloadTimer > m_Blackboard.m_ReloadTime;
    }
    private void Reload()
    {
        m_BulletManager.Reload();
        m_ReloadTimer = 0;
    }
}
