using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    public enum BulletType { NORMAL, ATTRACTOR, TELEPORT, MARK, STICKY, ICE, ENERGY }

    [Header("GENERICAL SHOOT SYSTEM")]
    public float m_BulletSpeed=2;
    [SerializeField] private float m_BulletLifetime=30f;
    public LayerMask m_ColisionWithEffect, m_ColisionLayerMask;

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
        switch (bulletType)
        {
            case BulletType.NORMAL:
                m_BulletList.Add(new NormalBullet(pos, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect));
                break;
            case BulletType.ATTRACTOR:
                m_BulletList.Add(new AttractorBullet(pos, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect, m_AttractorArea,m_AttractingTime,m_RequireAttractorDistance));
                break;
            case BulletType.TELEPORT:
                m_BulletList.Add(new TeleportBullet(pos, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect));
                break;
            case BulletType.MARK:
                break;
            case BulletType.STICKY:
                m_BulletList.Add(new StickyBullet(pos, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect, m_TimeToExplosion, m_ExplosionArea));
                break;
            case BulletType.ICE:
                m_BulletList.Add(new IceBullet(pos, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect, m_MaxIterations, m_TimeBetweenIteration, m_SlowSpeed));
                break;
            case BulletType.ENERGY:
                break;
            default:
                break;
        }
        m_BulletLifetimeList.Add(0.0f);
    }

    protected void UpdateShootSystem()
    {
        for (int i = 0; i < m_BulletList.Count; i++)
        {
            m_BulletLifetimeList[i] += Time.deltaTime;

            if (m_BulletList[i].Hit())
            {
                m_BulletList[i] = null;
                m_BulletList.RemoveAt(i);
                m_BulletLifetimeList.RemoveAt(i);
                --i;
            }
            else if (m_BulletLifetimeList[i] > m_BulletLifetime)
            {
                //if (m_BulletList[i] is AttractorBullet)
                //{
                //    Debug.Log("Bullet atttractor");
                //    m_BulletList[i].OnCollisionWithoutEffect();
                //}
                m_BulletList[i] = null;
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
}
