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

    [Header("ICE")]
    public int m_MaxIterations = 5;
    public float m_TimeBetweenIteration=1f;



    private float m_DamageBullet;
    private List<Bullet> m_BulletList = new List<Bullet>();
    private List<float> m_BulletLifetimeList = new List<float>();
    
    /// <summary>
    /// Create a bullet giving a position, direction/normal and speed
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="bulletType"></param>
    public void BulletShoot(Vector3 pos, Vector3 normal, float speed, BulletType bulletType)//, BulletType bulletType)
    {
        m_DamageBullet = damages[(int)bulletType];
        switch (bulletType)
        {
            case BulletType.NORMAL:
                m_BulletList.Add(new NormalBullet(pos, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect));
                break;
            case BulletType.ATTRACTOR:
               
                break;
            case BulletType.TELEPORT:
                m_BulletList.Add(new TeleportBullet(pos, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect));
                break;
            case BulletType.MARK:
                break;
            case BulletType.STICKY:
                break;
            case BulletType.ICE:
                m_BulletList.Add(new IceBullet(pos, normal, speed, m_DamageBullet, m_ColisionLayerMask, m_ColisionWithEffect, m_MaxIterations, m_TimeBetweenIteration));
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
