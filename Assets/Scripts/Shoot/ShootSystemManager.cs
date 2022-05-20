using System.Collections.Generic;
using UnityEngine;

public class ShootSystemManager : MonoBehaviour
{
    public enum BulletType { NORMAL, ATTRACTOR, TELEPORT, MARK, STICKY, ICE, ENERGY, DRONE }

    public Bullet[] bullets;
    [Header("GENERICAL SHOOT SYSTEM")]
    [HideInInspector] public float m_BulletSpeed = 2;
    [SerializeField] private float m_BulletLifetime = 20f;
    public LayerMask m_ColisionWithEffect, m_ColisionLayerMask;

    public float m_AngleDispersion = 9f;
    public float m_OffSetYValue = 0.1f;
    //public float m_

    [Tooltip("[0-Normal, 1-Attractor, 2-Teleport, 3-Mark, 4-Sticky, 5-Ice, 6-Energy] order reference.")]
    [SerializeField] private float[] m_BulletTypeDamages = new float[7];

    [Header("ICE")]
    public int m_MaxIterations = 5;
    public float m_TimeBetweenIteration = 1f;
    public float m_SlowSpeed = 3.5f;

    [Header("STICKY")]
    public float m_TimeToExplosion = 1f;

    [Header("ATTRACTOR")]
    public float m_AttractorArea = 5;
    public float m_AttractingTime = 2;
    public float m_RequireAttractorDistance = 0.5f;
    public GameObject m_ParticlesAttractor;

    [Header("TELEPORT")]
    public GameObject m_PlayerMesh;
    public GameObject m_TrailTeleport;
    public float m_VelocityPlayer = 10;
    public GameObject m_ParticlesTP;


    [Header("ENERGY")]
    public float m_SpeedEnergyBullet = 5f;

    private float m_DamageBullet;

    private void Start()
    {
        GameManager.GetManager().SetShootSystem(this);
    }

    public void BulletShoot(Transform pos, Vector3 normal, float speed, BulletType bulletType)
    {
        m_DamageBullet = m_BulletTypeDamages[(int)bulletType];
        Bullet l_CurrBullet = Instantiate(bullets[(int)bulletType],pos.position, Quaternion.identity);
        Debug.DrawLine(pos.position, normal * 10);
        switch (bulletType)
        {
            case BulletType.NORMAL:
                l_CurrBullet.SetBullet(pos.position, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                break;
            case BulletType.ATTRACTOR:
                l_CurrBullet.SetBullet(pos.position, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                l_CurrBullet.SetAttractor(m_AttractorArea, m_AttractingTime, m_RequireAttractorDistance, m_ParticlesAttractor);
                break;
            case BulletType.TELEPORT:
                l_CurrBullet.SetBullet(pos.position, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                l_CurrBullet.SetTeleport(m_PlayerMesh, m_TrailTeleport, m_VelocityPlayer, m_ParticlesTP);
                break;
            case BulletType.MARK:
                l_CurrBullet.SetBullet(pos.position, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                break;
            case BulletType.STICKY:
                l_CurrBullet.SetBullet(pos.position, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                l_CurrBullet.SetSticky(m_TimeToExplosion);
                break;
            case BulletType.ICE:
                l_CurrBullet.SetBullet(pos.position, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                l_CurrBullet.SetIce(m_MaxIterations, m_TimeBetweenIteration, m_SlowSpeed);
                break;
            case BulletType.ENERGY:
                //creating 4 extra bullets.
                List<EnergyBullet> l_EnergyBullets = new List<EnergyBullet>();

                Bullet l_DownBulletRight = Instantiate(bullets[(int)bulletType], pos.position, Quaternion.identity);
                Bullet l_DownBulletLeft = Instantiate(bullets[(int)bulletType], pos.position, Quaternion.identity);

                Bullet l_TopBullet = Instantiate(bullets[(int)bulletType], pos.position, Quaternion.identity);
                Bullet l_DownBullet = Instantiate(bullets[(int)bulletType], pos.position, Quaternion.identity);

                l_CurrBullet.SetBullet(pos.position, normal, m_SpeedEnergyBullet, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                l_DownBulletRight.SetBullet(pos.position, Quaternion.AngleAxis(m_AngleDispersion, GameManager.GetManager().GetCameraManager().transform.up) * normal, m_SpeedEnergyBullet, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                l_DownBulletLeft.SetBullet(pos.position, Quaternion.AngleAxis(m_AngleDispersion, -GameManager.GetManager().GetCameraManager().transform.up) * normal, m_SpeedEnergyBullet, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                l_TopBullet.SetBullet(pos.position, normal + new Vector3(0, m_OffSetYValue, 0), m_SpeedEnergyBullet, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                l_DownBullet.SetBullet(pos.position, normal + new Vector3(0, -m_OffSetYValue, 0), m_SpeedEnergyBullet, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);

                //add to energy list
                l_EnergyBullets.Add(l_DownBulletRight as EnergyBullet);
                l_EnergyBullets.Add(l_DownBulletLeft as EnergyBullet);
                l_EnergyBullets.Add(l_CurrBullet as EnergyBullet);

                l_EnergyBullets.Add(l_TopBullet as EnergyBullet);
                l_EnergyBullets.Add(l_DownBullet as EnergyBullet);

                //set energy
                l_DownBulletRight.SetEnegy(l_EnergyBullets);
                l_DownBulletLeft.SetEnegy(l_EnergyBullets);
                l_CurrBullet.SetEnegy(l_EnergyBullets);

                l_TopBullet.SetEnegy(l_EnergyBullets);
                l_DownBullet.SetEnegy(l_EnergyBullets);

                break;
            case BulletType.DRONE:
                l_CurrBullet.SetBullet(pos.position, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);

                break;
            default:
                break;
        }
    }
}
