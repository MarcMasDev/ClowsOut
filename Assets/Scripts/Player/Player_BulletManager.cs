using UnityEngine;
using static ShootSystem;

public class Player_BulletManager : MonoBehaviour
{
    public BulletType[] m_BulletList;
    private int m_BulletIndex;

    public BulletType m_CurrentBullet => m_BulletList[m_BulletIndex];
    public bool m_NoBullets => m_BulletIndex >= m_BulletList.Length;

    public void SetBullets(BulletType[] bulletList)
    {
        m_BulletList = bulletList;
    }
    public void NextBullet()
    {
        m_BulletIndex += 1;
    }
    public void Reload()
    {
        m_BulletIndex = 0;
    }
}
