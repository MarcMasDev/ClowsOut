using System;
using UnityEngine;
using static ShootSystemManager;

public class Player_BulletManager : MonoBehaviour, IRestart
{
    public static Action<int[]> OnChangeBullets;
    public static Action OnRotateClockwise,
        OnRotateCounterclockwise,
        OnShoot;

   // public BulletType[] m_UpdatableBulletList;

    private int[] m_BulletList = new int[3];

    private int m_ShootedBullets;
    public bool m_IsFull => m_ShootedBullets == 0;
    public bool m_NoBullets => m_ShootedBullets == 3;

    private static Player_BulletManager m_Instance = null;

    //TODO: All singlentons in the game controller

    public BulletType m_CurrentBullet => (BulletType) m_BulletList[0];

    //TODO: Move to input handle
    private void OnEnable()
    {
        GameManager.GetManager().GetInputManager().OnRotatingClockwise += UpdateRotateDrumClockwise;
        GameManager.GetManager().GetInputManager().OnRotatingCounterClockwise += UpdateRotateDrumCounterClockwise;
    }
    private void OnDisable()
    {
        GameManager.GetManager().GetInputManager().OnRotatingClockwise -= UpdateRotateDrumClockwise;
        GameManager.GetManager().GetInputManager().OnRotatingCounterClockwise -= UpdateRotateDrumCounterClockwise;
    }
    private void Start()
    {
        GameManager.GetManager().SetPlayerBulletManager(this);
        SetBulletList(GameManager.GetManager().GetLevelData().LoadDataPlayerBullets());
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
        SetBulletList(GameManager.GetManager().GetLevelData().LoadDataPlayerBullets());
        OnChangeBullets?.Invoke(m_BulletList);
        ManagerUI.m_BulletHUDActualized = false;
    }

    public void AddRestartElement()
    {
        GameManager.GetManager().GetRestartManager().addRestartElement(this,transform);
    }

    public void Restart()
    {
        SetBulletList(GameManager.GetManager().GetLevelData().LoadDataPlayerBullets());
        OnChangeBullets?.Invoke(m_BulletList);
        ManagerUI.m_BulletHUDActualized = false;
    }
    public void SetBulletList(BulletType[] bulletTypes)
    {
        for (int i = 0; i < bulletTypes.Length; i++)
        {
            m_BulletList[i] = (int)bulletTypes[i];
        }

        GameManager.GetManager().GetLevelData().SaveDataPlayerBullets(bulletTypes);
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
