using System;
using UnityEngine;

[RequireComponent(typeof(Player_InputHandle))]
public class Player_ShootSystem : MonoBehaviour
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
    private Player_Blackboard m_Blackboard;
    private bool m_UpdateReload = false;

    void Awake()
    {
        m_Input = GetComponent<Player_InputHandle>();
        m_Dispersion = GetComponent<Player_Dispersion>();
        m_Blackboard = GetComponent<Player_Blackboard>();
        m_RateOfFireTimer = m_Blackboard.m_RateOfFire;
        m_ReloadTimer = m_Blackboard.m_ReloadTime;
        m_ShootTimer = m_Blackboard.m_ShootTime;
    }

    void Update()
    {
        if (!m_Blackboard.m_Death)
        {
            RaycastHit l_Hit;
            if (Physics.Raycast(GameManager.GetManager().GetCameraManager().m_Camera.transform.position,
                GameManager.GetManager().GetCameraManager().m_Camera.transform.forward,
                out l_Hit, m_Blackboard.m_AimMaxDistance, m_Blackboard.m_AimLayers))
            {
                m_Blackboard.m_AimTarget.transform.position = m_Blackboard.m_ShootPoint.transform.position +
                    GameManager.GetManager().GetCameraManager().m_Camera.transform.forward * m_Blackboard.m_AimMaxDistance;
            }
            if (CanShoot()) //&& m_Blackboard.m_Teleported)
            {
                Shoot();
                m_Input.Shooting = false;
            }
            else
            {
                m_Input.Shooting = false;
                m_ContinuousBulletsFired = 0;
            }
            m_RateOfFireTimer += Time.deltaTime;
            m_ShootTimer += Time.deltaTime;

            if (CanAutomaticReload())
            {
                //TODO: Sound / Animation / Change Hud (ammo)
                Reload();
                m_UpdateReload = true;
                m_Input.Reloading = false;
            }
            else
            {
                m_Input.Reloading = false;
            }

            if (CanUpdateReload())
            {
                GameManager.GetManager().GetPlayerBulletManager().Reload();
                m_UpdateReload = false;
            }

            m_ReloadTimer += Time.deltaTime;

            //updating bullets of shootsystem 
            /*
             * TODO:
            if (m_ShootTimer > m_ShootTime && m_ReloadTimer > m_ReloadTime)
            {
            maybe idle aiming animation
            }
            */
        }
    }
    private bool CanShoot()
    {
        return m_Input.Shooting && m_RateOfFireTimer >= m_Blackboard.m_RateOfFire && m_ReloadTimer 
            >= m_Blackboard.m_ReloadTime && !GameManager.GetManager().GetPlayerBulletManager().m_NoBullets && ManagerUI.m_BulletHUDActualized
            && !m_Blackboard.m_OnWall;
    }
    private void Shoot()
    {
        GameManager.GetManager().GetLevelData().SaveBulletsUsed();

        m_CurrentDispersion = m_Dispersion.m_CurrentDispersion;
        m_CurrentDispersion *= Mathf.Deg2Rad;

        CreateBullet();

        OnShoot?.Invoke();
        GameManager.GetManager().GetCameraShake().Shake(m_Blackboard.m_ShootIntensity, 0, null);
        //m_Blackboard.m_FMODDolores.Shoot();
        //TODO: fireFX / Sound / Animation / Change Hud (ammo)
        m_ContinuousBulletsFired += 1;
        m_RateOfFireTimer = 0;
        m_ShootTimer = 0;
    }
    private void CreateBullet()
    {
        //TODO: Ainoa Shoot System
        RaycastHit l_Hit;
        if (Physics.Raycast(GameManager.GetManager().GetCameraManager().m_Camera.transform.position, GameManager.GetManager().GetCameraManager().m_Camera.transform.forward, out l_Hit, m_Blackboard.m_AimMaxDistance, m_Blackboard.m_AimLayers))
        {
            m_AimPoint = l_Hit.point;
        }
        else
        {
            m_AimPoint = m_Blackboard.m_ShootPoint.transform.position + GameManager.GetManager().GetCameraManager().m_Camera.transform.forward * m_Blackboard.m_AimMaxDistance;
        }
        Vector3 l_AimNormal = (m_AimPoint - m_Blackboard.m_ShootPoint.transform.position).normalized;
        Vector3 l_BulletNormal = (l_AimNormal + BulletDispersion()).normalized;

      //  Ray l_Ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
       

        //temporal type bullet var
        //m_ShootSystem.BulletShoot(m_Blackboard.m_ShootPoint.position, l_BulletNormal, m_Blackboard.m_BulletSpeed, GameManager.GetManager().GetPlayerBulletManager().m_CurrentBullet);
        GameManager.GetManager().GetShootSystemManager().BulletShoot(m_Blackboard.m_ShootPoint.position, l_BulletNormal, 
            m_Blackboard.m_BulletSpeed, GameManager.GetManager().GetPlayerBulletManager().m_CurrentBullet, m_Blackboard.m_CollisionWithEffect, m_Blackboard.m_AimLayers);
        GameManager.GetManager().GetPlayerBulletManager().NextBullet();
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
        return m_ShootTimer > m_Blackboard.m_ShootTime && m_ReloadTimer > m_Blackboard.m_ReloadTime 
            && !GameManager.GetManager().GetPlayerBulletManager().m_IsFull && (m_Input.Reloading || GameManager.GetManager().GetPlayerBulletManager().m_NoBullets) 
            && !m_UpdateReload && !m_Blackboard.m_OnWall;
    }
    private bool CanUpdateReload()
    {
        return m_ShootTimer > m_Blackboard.m_ShootTime && m_ReloadTimer > m_Blackboard.m_ReloadTime
            && m_UpdateReload && !m_Blackboard.m_OnWall;
    }
    private void Reload()
    {
        m_ReloadTimer = 0;
        m_Blackboard.m_Animator.SetTrigger("Reload");
    }
}
