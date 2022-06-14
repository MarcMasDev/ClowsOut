using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FMODDolores : MonoBehaviour
{
    [SerializeField] private Player_BulletManager m_BulletManager;
    [SerializeField] private Player_ShootSystem m_ShootSystem;
    [SerializeField] private Player_FSM m_PlayerFSM;

    [SerializeField] private StudioEventEmitter m_Footstep;
    [SerializeField] private StudioEventEmitter m_Hit;
    [SerializeField] private StudioEventEmitter m_ShootNormal;
    [SerializeField] private StudioEventEmitter m_ShootSpecial;
    [SerializeField] private StudioEventEmitter m_Dash;
    [SerializeField] private StudioEventEmitter m_Reload;
    private int m_ShootedBullet;
    private void OnEnable()
    {
        m_ShootSystem.OnShoot += BulletShoot;
        m_PlayerFSM.OnDash += Dash;
    }
    private void OnDisable()
    {
        m_ShootSystem.OnShoot -= BulletShoot;
        m_PlayerFSM.OnDash -= Dash;
    }
    public void FootStep()
    {
        //Sound
        m_Footstep?.Play();
    }
    public void Shoot()
    {
        //Sound
        if (m_ShootedBullet == 1 || m_ShootedBullet == 2 ||
            m_ShootedBullet == 3 || m_ShootedBullet == 6)
        {
            m_ShootSpecial?.Play();
        }
        else
        {
            m_ShootNormal?.Play();
        }
    }
    public void Hit()
    {
        m_Hit?.Play();
    }
    public void Dash()
    {
        m_Dash?.Play();
    }
    public void Reload()
    {
        m_Reload?.Play();
    }
    private void BulletShoot()
    {
        //NORMAL, ATTRACTOR, TELEPORT, MARK, STICKY, ICE, ENERGY, DRONE
        //TEleport/Energy/Attractor
        m_ShootedBullet = (int)m_BulletManager.m_CurrentBullet;
    }
}
