using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    //TODO: 1 ShootSystem some var can be changed from the shooter
    public enum BulletType { NORMAL, ATTRACTOR, TELEPORT, MARK, STICKY, ICE, ENERGY }
    public Bullet[] bullets;
    [Header("GENERICAL SHOOT SYSTEM")]
    public float m_BulletSpeed=2;
    [SerializeField] private float m_BulletLifetime=30f;
    public LayerMask m_ColisionWithEffect, m_ColisionLayerMask;

    public float m_AngleDispersion=9f;
    public float m_OffSetYValue= 0.1f;
    //public float m_

    [Tooltip("[0-Normal, 1-Attractor, 2-Teleport, 3-Mark, 4-Sticky, 5-Ice, 6-Energy] order reference.")]
    [SerializeField] private float[] m_BulletTypeDamages = new float[7];

    [Header("ICE")]
    public int m_MaxIterations = 5;
    public float m_TimeBetweenIteration=1f;
    public float m_SlowSpeed = 3.5f;

    [Header("STICKY")]
    public float m_TimeToExplosion = 1f;
    public float m_ExplosionArea=4;

    [Header("ATTRACTOR")]
    public float m_AttractorArea=5;
    public float m_AttractingTime=2;
    public float m_RequireAttractorDistance = 0.5f;

    [Header("TELEPORT")]
    public GameObject m_PlayerMesh;
    public GameObject m_TrailTeleport;

    [Header("ENERGY")]
    public float m_SpeedEnergyBullet=5f;

    private float m_DamageBullet;
    private List<Bullet> m_BulletList = new List<Bullet>();
    private List<float> m_BulletLifetimeList = new List<float>();

    /// <summary>
    /// Create a bullet giving a position, direction/normal, speed and type of bullet.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="normal"></param>
    /// <param name="speed"></param>
    /// <param name="bulletType"></param>
    public void BulletShoot(Vector3 pos, Vector3 normal, float speed, BulletType bulletType)
    {
        m_DamageBullet = m_BulletTypeDamages[(int)bulletType];
        Bullet currBullet = Instantiate(bullets[(int)bulletType],transform.position,Quaternion.identity);
        
        switch (bulletType)
        {
            case BulletType.NORMAL:
                currBullet.SetBullet(pos, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                break;
            case BulletType.ATTRACTOR:
                currBullet.SetBullet(pos, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                currBullet.SetAttractor(m_AttractorArea, m_AttractingTime, m_RequireAttractorDistance);
                break;
            case BulletType.TELEPORT:
                currBullet.SetBullet(pos, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                currBullet.SetTeleport(m_PlayerMesh, m_TrailTeleport);
                break;
            case BulletType.MARK:
                break;
            case BulletType.STICKY:
                currBullet.SetBullet(pos, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                currBullet.SetSticky(m_TimeToExplosion, m_ExplosionArea);
                break;
            case BulletType.ICE:
                currBullet.SetBullet(pos, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                currBullet.SetIce(m_MaxIterations, m_TimeBetweenIteration, m_SlowSpeed);
                break;
            case BulletType.ENERGY:
                //creating 2 extra bullets.
                List<EnergyBullet> l_EnergyBullets = new List<EnergyBullet>();

                Bullet extraBullet1 = Instantiate(bullets[(int)bulletType], transform.position, Quaternion.identity);
                Bullet extraBullet2 = Instantiate(bullets[(int)bulletType], transform.position, Quaternion.identity);
               
                currBullet.SetBullet(pos, Quaternion.AngleAxis(m_AngleDispersion, CameraManager.Instance.transform.up) * normal, m_SpeedEnergyBullet, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                extraBullet1.SetBullet(pos, Quaternion.AngleAxis(m_AngleDispersion, -CameraManager.Instance.transform.up) * normal, m_SpeedEnergyBullet, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);
                extraBullet2.SetBullet(pos, normal + new Vector3(0, m_OffSetYValue, 0), m_SpeedEnergyBullet, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect);

                l_EnergyBullets.Add(extraBullet1 as EnergyBullet);
                l_EnergyBullets.Add(extraBullet2 as EnergyBullet);
                l_EnergyBullets.Add(currBullet as EnergyBullet);

                extraBullet1.SetEnegy(l_EnergyBullets);
                extraBullet2.SetEnegy(l_EnergyBullets);
                currBullet.SetEnegy(l_EnergyBullets);

                m_BulletList.Add(extraBullet1);
                m_BulletList.Add(extraBullet2);
                m_BulletLifetimeList.Add(0.0f);
                m_BulletLifetimeList.Add(0.0f);
                break;
            default:
                break;
        }
        m_BulletList.Add(currBullet);
        m_BulletLifetimeList.Add(0.0f);
    }

    //TODO: Pooling
    protected void UpdateShootSystem()
    {
        for (int i = 0; i < m_BulletList.Count; i++)
        {
            m_BulletLifetimeList[i] += Time.deltaTime;

            if (m_BulletList[i].Hit())
            {
                m_BulletList.RemoveAt(i);
                m_BulletLifetimeList.RemoveAt(i);
                --i;
            }
            else if (m_BulletLifetimeList[i] > m_BulletLifetime)
            {
                Destroy(m_BulletList[i].gameObject);
                m_BulletList.RemoveAt(i);
                m_BulletLifetimeList.RemoveAt(i);
                --i;
            }
            else
            {
                m_BulletList[i].Move();
            }
        }
    }
    private void Update()
    {
        UpdateShootSystem();
    }
}
