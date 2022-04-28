using System;
using UnityEngine;
using static ShootSystem;

public class Player_BulletManager : MonoBehaviour, IRestart
{
    public static Action<BulletType[]> OnUpdateHUDBulletList;
    public static Action<BulletType[], int> OnUpdateHUDNextBullet;

    [SerializeField] public BulletType[] m_InitialBulletList;
    public BulletType[] m_BulletList;

    private static Player_BulletManager m_Instance = null;

    //TODO: All singlentons in the game controller
    public static Player_BulletManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType<Player_BulletManager>();
            }
            return m_Instance;
        }
    }

    private int m_BulletIndex;

    public BulletType m_CurrentBullet => m_BulletList[m_BulletIndex];
    public bool m_NoBullets => m_BulletIndex >= m_BulletList.Length;
    public bool m_IsFull => m_BulletIndex == 0;

    private void Start()
    {
        OnUpdateHUDBulletList?.Invoke(m_InitialBulletList);
        m_BulletList = m_InitialBulletList;
        AddRestartElement();
    }

    public void SetBullets(BulletType[] bulletList)
    {
        m_InitialBulletList = bulletList;
    }
    public void NextBullet()
    {
        m_BulletIndex += 1;
        OnUpdateHUDNextBullet?.Invoke(m_InitialBulletList, m_BulletIndex);
    }
    public void Reload()
    {
        m_BulletIndex = 0;
        OnUpdateHUDBulletList?.Invoke(m_InitialBulletList);
    }

    public void AddRestartElement()
    {
        RestartElements.m_Instance.addRestartElement(this);
    }

    public void Restart()
    {
        OnUpdateHUDBulletList?.Invoke(m_InitialBulletList);
        m_BulletList = m_InitialBulletList;
    }
}
