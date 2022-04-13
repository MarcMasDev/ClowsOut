using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    //public enum BulletType { ATTRACTOR = 0, LINKED, EXPLOSIVE, KNOCKBACK, FIRE, TELEPORT }
    //public BulletType m_CurrBulletType;
    [Header("GENERICAL SHOOT SYSTEM")]
    public float m_BulletSpeed=2;
    public LayerMask m_ColisionWithEffect, m_ColisionLayerMask;

    private List<Bullet> m_BulletList = new List<Bullet>();
    private List<float> m_BulletLifetimeList = new List<float>();
    [SerializeField ]private float m_BulletLifetime;


    /// <summary>
    /// Create a bullet giving a direction and speed
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="bulletType"></param>
    public void BulletShoot(Vector3 pos, Vector3 normal, float speed)//, BulletType bulletType)
    {
        
        m_BulletList.Add(new Bullet(pos, normal, speed, m_ColisionLayerMask,m_ColisionWithEffect));
        m_BulletLifetimeList.Add(0.0f);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    BulletShoot(new Vector3(0, 0, 1), m_BulletSpeed);//, ShootSystem.BulletType.ATTRACTOR);
        //}

        UpdateShootSystem();
    }

    public void UpdateShootSystem()
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
                m_BulletList[i].Move();
        }


    }
}
