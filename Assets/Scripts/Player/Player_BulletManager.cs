using System;
using UnityEngine;
using static ShootSystem;

public class Player_BulletManager : MonoBehaviour
{
    public static Action<BulletType[]> OnUpdateHUDBulletList;
    public static Action<BulletType[], int> OnUpdateHUDNextBullet;

    public BulletType[] m_BulletList;
    private int m_BulletIndex;

    public BulletType m_CurrentBullet => m_BulletList[m_BulletIndex];
    public bool m_NoBullets => m_BulletIndex >= m_BulletList.Length;

    private void Start()
    {
        OnUpdateHUDBulletList?.Invoke(m_BulletList);
    }

    public void SetBullets(BulletType[] bulletList)
    {
        m_BulletList = bulletList;
    }
    public void NextBullet()
    {
        m_BulletIndex += 1;
        OnUpdateHUDNextBullet?.Invoke(m_BulletList, m_BulletIndex);
        Debug.Log("NextBullet: " + m_BulletIndex);
    }
    public void Reload()
    {
        m_BulletIndex = 0;
        OnUpdateHUDBulletList?.Invoke(m_BulletList);
        Debug.Log("Reload");
    }
}
