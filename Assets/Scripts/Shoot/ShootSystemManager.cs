using System.Collections.Generic;
using UnityEngine;

public class ShootSystemManager : MonoBehaviour
{
    public enum BulletType { NORMAL, ATTRACTOR, TELEPORT, MARK, STICKY, ICE, ENERGY, DRONE }

    public Bullet[] bullets;
    [Header("GENERICAL SHOOT SYSTEM")]
    [HideInInspector] public float m_BulletSpeed = 2;
    [SerializeField] private float m_BulletLifetime = 20f;

    public float m_AngleDispersion = 9f;
    public float m_OffSetYValue = 0.1f;

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
    public PlayParticle m_ParticlesTP;

    [Header("ENERGY")]
    public float m_SpeedEnergyBullet = 5f;
    private float m_DamageBullet;

    private void Awake()
    {
        GameManager.GetManager().SetShootSystem(this);
    }

    /// <summary>
    /// Create a Bullet. Default method.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="normal"></param>
    /// <param name="speed"></param>
    /// <param name="bulletType"></param>
    /// <param name="colisionWithEffect"></param>
    /// <param name="colisionLayerMask"></param>
    public void BulletShoot(Vector3 pos, Vector3 normal, float speed, BulletType bulletType, LayerMask colisionWithEffect, LayerMask colisionLayerMask)
    {
        m_DamageBullet = m_BulletTypeDamages[(int)bulletType];
        Bullet l_CurrBullet = Instantiate(bullets[(int)bulletType],pos, Quaternion.identity);
        switch (bulletType)
        {
            case BulletType.NORMAL:
                l_CurrBullet.SetBullet(pos, normal, speed, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                break;
            case BulletType.ATTRACTOR:
                l_CurrBullet.SetBullet(pos, normal, speed, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_CurrBullet.SetAttractor(m_AttractorArea, m_AttractingTime, m_RequireAttractorDistance, m_ParticlesAttractor);
                break;
            case BulletType.TELEPORT:
                l_CurrBullet.SetBullet(pos, normal, speed, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_CurrBullet.SetTeleport(m_PlayerMesh, m_TrailTeleport, m_VelocityPlayer, m_ParticlesTP);
                break;
            case BulletType.MARK:
                l_CurrBullet.SetBullet(pos, normal, speed, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                break;
            case BulletType.STICKY:
                l_CurrBullet.SetBullet(pos, normal, speed, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_CurrBullet.SetSticky(m_TimeToExplosion);
                break;
            case BulletType.ICE:
                l_CurrBullet.SetBullet(pos, normal, speed, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_CurrBullet.SetIce(m_MaxIterations, m_TimeBetweenIteration, m_SlowSpeed);
                break;
            case BulletType.ENERGY:
                //creating 4 extra bullets.
                List<EnergyBullet> l_EnergyBullets = new List<EnergyBullet>();

                Bullet l_DownBulletRight = Instantiate(bullets[(int)bulletType], pos, Quaternion.identity);
                Bullet l_DownBulletLeft = Instantiate(bullets[(int)bulletType], pos, Quaternion.identity);

                Bullet l_TopBullet = Instantiate(bullets[(int)bulletType], pos, Quaternion.identity);
                Bullet l_DownBullet = Instantiate(bullets[(int)bulletType], pos, Quaternion.identity);

                l_CurrBullet.SetBullet(pos, normal, m_SpeedEnergyBullet, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_DownBulletRight.SetBullet(pos, Quaternion.AngleAxis(m_AngleDispersion, GameManager.GetManager().GetCameraManager().transform.up) * normal, m_SpeedEnergyBullet, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_DownBulletLeft.SetBullet(pos, Quaternion.AngleAxis(m_AngleDispersion, -GameManager.GetManager().GetCameraManager().transform.up) * normal, m_SpeedEnergyBullet, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_TopBullet.SetBullet(pos, normal + new Vector3(0, m_OffSetYValue, 0), m_SpeedEnergyBullet, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_DownBullet.SetBullet(pos, normal + new Vector3(0, -m_OffSetYValue, 0), m_SpeedEnergyBullet, m_DamageBullet, colisionLayerMask, colisionWithEffect);

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
                l_CurrBullet.SetBullet(pos, normal, speed, m_DamageBullet, colisionLayerMask, colisionWithEffect);

                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Create a Bullet sending damage variable
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="normal"></param>
    /// <param name="speed"></param>
    /// <param name="damage"></param>
    /// <param name="bulletType"></param>
    /// <param name="colisionWithEffect"></param>
    /// <param name="colisionLayerMask"></param>
    public void BulletShoot(Vector3 pos, Vector3 normal, float speed, float damage, BulletType bulletType, LayerMask colisionWithEffect, LayerMask colisionLayerMask)
    {
        m_DamageBullet = damage;
        Bullet l_CurrBullet = Instantiate(bullets[(int)bulletType], pos, Quaternion.identity);
        switch (bulletType)
        {
            case BulletType.NORMAL:
                l_CurrBullet.SetBullet(pos, normal, speed, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                break;
            case BulletType.ATTRACTOR:
                l_CurrBullet.SetBullet(pos, normal, speed, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_CurrBullet.SetAttractor(m_AttractorArea, m_AttractingTime, m_RequireAttractorDistance, m_ParticlesAttractor);
                break;
            case BulletType.TELEPORT:
                l_CurrBullet.SetBullet(pos, normal, speed, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_CurrBullet.SetTeleport(m_PlayerMesh, m_TrailTeleport, m_VelocityPlayer, m_ParticlesTP);
                break;
            case BulletType.MARK:
                l_CurrBullet.SetBullet(pos, normal, speed, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                break;
            case BulletType.STICKY:
                l_CurrBullet.SetBullet(pos, normal, speed, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_CurrBullet.SetSticky(m_TimeToExplosion);
                break;
            case BulletType.ICE:
                l_CurrBullet.SetBullet(pos, normal, speed, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_CurrBullet.SetIce(m_MaxIterations, m_TimeBetweenIteration, m_SlowSpeed);
                break;
            case BulletType.ENERGY:
                //creating 4 extra bullets.
                List<EnergyBullet> l_EnergyBullets = new List<EnergyBullet>();

                Bullet l_DownBulletRight = Instantiate(bullets[(int)bulletType], pos, Quaternion.identity);
                Bullet l_DownBulletLeft = Instantiate(bullets[(int)bulletType], pos, Quaternion.identity);

                Bullet l_TopBullet = Instantiate(bullets[(int)bulletType], pos, Quaternion.identity);
                Bullet l_DownBullet = Instantiate(bullets[(int)bulletType], pos, Quaternion.identity);

                l_CurrBullet.SetBullet(pos, normal, m_SpeedEnergyBullet, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_DownBulletRight.SetBullet(pos, Quaternion.AngleAxis(m_AngleDispersion, GameManager.GetManager().GetCameraManager().transform.up) * normal, m_SpeedEnergyBullet, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_DownBulletLeft.SetBullet(pos, Quaternion.AngleAxis(m_AngleDispersion, -GameManager.GetManager().GetCameraManager().transform.up) * normal, m_SpeedEnergyBullet, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_TopBullet.SetBullet(pos, normal + new Vector3(0, m_OffSetYValue, 0), m_SpeedEnergyBullet, m_DamageBullet, colisionLayerMask, colisionWithEffect);
                l_DownBullet.SetBullet(pos, normal + new Vector3(0, -m_OffSetYValue, 0), m_SpeedEnergyBullet, m_DamageBullet, colisionLayerMask, colisionWithEffect);

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
                l_CurrBullet.SetBullet(pos, normal, speed, m_DamageBullet, colisionLayerMask, colisionWithEffect);

                break;
            default:
                break;
        }
    }
}
