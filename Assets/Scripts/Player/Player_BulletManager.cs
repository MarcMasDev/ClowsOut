using System;
using UnityEngine;
using static ShootSystem;

public class Player_BulletManager : MonoBehaviour, IRestart
{
    public static Action<int[]> OnChangeBullets;
    public static Action OnRotateClockwise,
        OnRotateCounterclockwise,
        OnShoot;

    public BulletType[] m_UpdatableBulletList;

    private int[] m_BulletList = new int[3];

    private int m_ShootedBullets;
    public bool m_IsFull => m_ShootedBullets == 0;
    public bool m_NoBullets => m_ShootedBullets == 3;

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

    public BulletType m_CurrentBullet => (BulletType) m_BulletList[0];

    //TODO: Move to input handle
    private void OnEnable()
    {
        InputManager.Instance.OnRotatingClockwise += UpdateRotateDrumClockwise;
        InputManager.Instance.OnRotatingCounterClockwise += UpdateRotateDrumCounterClockwise;
    }
    private void OnDisable()
    {
        InputManager.Instance.OnRotatingClockwise -= UpdateRotateDrumClockwise;
        InputManager.Instance.OnRotatingCounterClockwise -= UpdateRotateDrumCounterClockwise;
    }
    private void Start()
    {
        SetBulletList(m_UpdatableBulletList);
        OnChangeBullets?.Invoke(m_BulletList);
        AddRestartElement();
        ManagerUI.m_BulletHUDActualized = false;
    }
    public void NextBullet()
    {
        m_ShootedBullets++;
        m_BulletList[0] = -1;
        OnShoot?.Invoke();
        UpdateRotateDrumClockwise();
    }
    public void Reload()
    {
        m_ShootedBullets = 0;
        SetBulletList(m_UpdatableBulletList);
        OnChangeBullets?.Invoke(m_BulletList);
        ManagerUI.m_BulletHUDActualized = false;
    }

    public void AddRestartElement()
    {
        RestartElements.m_Instance.addRestartElement(this);
    }

    public void Restart()
    {
        SetBulletList(m_UpdatableBulletList);
        OnChangeBullets?.Invoke(m_BulletList);
        ManagerUI.m_BulletHUDActualized = false;
    }
    public void SetBulletList(BulletType[] bulletTypes)
    {
        for (int i = 0; i < bulletTypes.Length; i++)
        {
            m_BulletList[i] = (int)bulletTypes[i];
        }
    }
    public void RotateDrumClockwise()
    {
        int[] l_NewBulletList = (int[])m_BulletList.Clone();
        m_BulletList[0] = l_NewBulletList[1];
        m_BulletList[1] = l_NewBulletList[2];
        m_BulletList[2] = l_NewBulletList[0];
        OnRotateClockwise?.Invoke();
    }
    public void RotateDrumCounterClockwise()
    {
        int[] l_NewBulletList = (int[])m_BulletList.Clone();
        m_BulletList[0] = l_NewBulletList[2];
        m_BulletList[1] = l_NewBulletList[0];
        m_BulletList[2] = l_NewBulletList[1];
        OnRotateCounterclockwise?.Invoke();
    }
    public void UpdateRotateDrumClockwise()
    {
        if (ManagerUI.m_BulletHUDActualized)
        {
            if (!m_NoBullets)
            {
                RotateDrumClockwise();
                for (int i = 0; i < m_BulletList.Length; i++)
                {
                    if (m_BulletList[0] != -1)
                    {
                        i = m_BulletList.Length;
                    }
                    else
                    {
                        RotateDrumClockwise();
                    }
                }
            }
            ManagerUI.m_BulletHUDActualized = false;
        }
    }
    public void UpdateRotateDrumCounterClockwise()
    {
        if (ManagerUI.m_BulletHUDActualized)
        {
            if (!m_NoBullets)
            {
                RotateDrumCounterClockwise();
                for (int i = 0; i < m_BulletList.Length; i++)
                {
                    if (m_BulletList[0] != -1)
                    {
                        i = m_BulletList.Length;
                    }
                    else
                    {
                        RotateDrumCounterClockwise();
                    }
                }
            }
            ManagerUI.m_BulletHUDActualized = false;
        }
    }
}
