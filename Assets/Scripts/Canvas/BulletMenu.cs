using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static ShootSystem;

public class BulletMenu : MonoBehaviour
{
    public TMP_Text[] m_SelectableBulletsText;
    public TMP_Text[] m_EquippedBulletsText;
    public Image[] m_EquippedBulletsImage;
    public Color m_UnequippedColor;
    public Color m_EquippedColor;

    private BulletType[] m_EquippedBulletsType = new BulletType[3];

    public BulletUI m_BulletUI;
    void Start()
    {
        InitBulletMenu();
    }
    private void InitBulletMenu()
    {
        for (int i = 0; i < m_SelectableBulletsText.Length; i++)
        {
            m_SelectableBulletsText[i].text = m_BulletUI.BulletTypeToName(i);
        }

        for (int i = 0; i < m_EquippedBulletsText.Length; i++)
        {
            m_EquippedBulletsText[i].text = "";
            m_EquippedBulletsImage[i].color = m_UnequippedColor;
        }
    }
    public void EquipBullet(int n)
    {
        for (int i = 0; i < m_EquippedBulletsType.Length; i++)
        {
            if (m_EquippedBulletsType[i] == default)
            {
                m_EquippedBulletsType[i] = (BulletType)n;
                m_EquippedBulletsText[i].text = m_BulletUI.BulletTypeToName(i);
                m_EquippedBulletsImage[i].color = m_EquippedColor;
            }
        }
        
    }
}
