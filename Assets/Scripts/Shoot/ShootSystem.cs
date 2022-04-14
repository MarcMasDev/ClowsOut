using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    public enum BulletType { NORMAL, ATTRACTOR, TELEPORT, MARK, STICKY, ICE, ENERGY }

    [Header("GENERICAL SHOOT SYSTEM")]
    public float m_BulletSpeed=2;
    [SerializeField] private float m_BulletLifetime;
    public LayerMask m_ColisionWithEffect, m_ColisionLayerMask;

    [Tooltip("[0-Normal, 1-Attractor, 2-Teleport, 3-Mark, 4-Sticky, 5-Ice, 6-Energy] order reference.")]
    public float[] damages;
    
    private float m_DamageBulletAverage;
    private List<Bullet> m_BulletList = new List<Bullet>();
    private List<float> m_BulletLifetimeList = new List<float>();
    
    /// <summary>
    /// Create a bullet giving a position, direction/normal and speed
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="bulletType"></param>
    public void BulletShoot(Vector3 pos, Vector3 normal, float speed, BulletType bulletType)//, BulletType bulletType)
    {
        switch (bulletType)
        {
            case BulletType.NORMAL:
                m_DamageBulletAverage = damages[(int)bulletType];
                m_BulletList.Add(new NormalBullet(pos, normal, speed, m_DamageBulletAverage, m_ColisionLayerMask, m_ColisionWithEffect));
                break;
            case BulletType.ATTRACTOR:
                m_DamageBulletAverage = damages[(int)bulletType];
                break;
            case BulletType.TELEPORT:
                m_DamageBulletAverage = damages[(int)bulletType];
                m_BulletList.Add(new TeleportBullet(pos, normal, speed, m_DamageBulletAverage, m_ColisionLayerMask, m_ColisionWithEffect));
                break;
            case BulletType.MARK:
                m_DamageBulletAverage = damages[(int)bulletType];
                break;
            case BulletType.STICKY:
                m_DamageBulletAverage = damages[(int)bulletType];
                break;
            case BulletType.ICE:
                m_DamageBulletAverage = damages[(int)bulletType];
                break;
            case BulletType.ENERGY:
                m_DamageBulletAverage = damages[(int)bulletType];
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
