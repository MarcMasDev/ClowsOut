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

    public BulletUI m_BulletUI;

    private void OnEnable()
    {
        //Player_BulletManager.OnUpdateHUDBulletList += UpdateHUDBulletList;
        //Player_BulletManager.OnUpdateHUDNextBullet += UpdateHUDNextBullet;
    }
    private void OnDisable()
    {
        //Player_BulletManager.OnUpdateHUDBulletList -= UpdateHUDBulletList;
        //Player_BulletManager.OnUpdateHUDNextBullet -= UpdateHUDNextBullet;
    }
    void UpdateHUDBulletList(BulletType[] bulletList)
    {
        for (int i = 0; i < bulletList.Length; i++)
        {
            m_BulletImage[i].color = m_UnusedColor;
            m_BulletText[i].text = m_BulletUI.BulletTypeToName((int)bulletList[i]);
        }
    }

    void UpdateHUDNextBullet(BulletType[] bulletList, int bulletIndex)
    {
        m_BulletImage[bulletList.Length - bulletIndex].color = m_UsedColor;
        m_BulletText[bulletList.Length - bulletIndex].text = "";

        for (int i = 0; i < bulletList.Length - bulletIndex; i++)
        {
            m_BulletText[i].text = m_BulletUI.BulletTypeToName((int)bulletList[i + bulletIndex]);
        }
    }
}
