using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ShootSystem;
using TMPro;
using System;

public class BulletsHUD : MonoBehaviour
{
    public Image[] m_BulletImage;
    public TMP_Text[] m_BulletText;

    public Color m_UnusedColor;
    public Color m_UsedColor;

    public string[] m_BulletTypesNames = new string[7];
    private void OnEnable()
    {
        Player_BulletManager.OnUpdateHUDBulletList += UpdateHUDBulletList;
        Player_BulletManager.OnUpdateHUDNextBullet += UpdateHUDNextBullet;
    }
    private void OnDisable()
    {
        Player_BulletManager.OnUpdateHUDBulletList -= UpdateHUDBulletList;
        Player_BulletManager.OnUpdateHUDNextBullet -= UpdateHUDNextBullet;
    }
    void UpdateHUDBulletList(BulletType[] bulletList)
    {
        for (int i = 0; i < bulletList.Length; i++)
        {
            m_BulletImage[i].color = m_UnusedColor;
            m_BulletText[i].text = BulletTypeToName(bulletList[i]);
        }
    }

    private string BulletTypeToName(BulletType bulletType)
    {
        switch (bulletType)
        {
            case BulletType.NORMAL:
                return m_BulletTypesNames[0];
            case BulletType.ATTRACTOR:
                return m_BulletTypesNames[1];
            case BulletType.TELEPORT:
                return m_BulletTypesNames[2];
            case BulletType.MARK:
                return m_BulletTypesNames[3];
            case BulletType.STICKY:
                return m_BulletTypesNames[4];
            case BulletType.ICE:
                return m_BulletTypesNames[5];
            case BulletType.ENERGY:
                return m_BulletTypesNames[6];
            default:
                return null;
        }
    }

    void UpdateHUDNextBullet(BulletType[] bulletList, int bulletIndex)
    {
        m_BulletImage[bulletList.Length - bulletIndex].color = m_UsedColor;
        m_BulletText[bulletList.Length - bulletIndex].text = "";

        for (int i = 0; i < bulletList.Length - bulletIndex; i++)
        {
            m_BulletText[i].text = BulletTypeToName(bulletList[i + bulletIndex]);
        }
    }
}
