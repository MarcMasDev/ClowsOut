using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShootSystem;

public class BulletsHUD2 : MonoBehaviour
{
    public Image[] m_BulletImage;
    public TMP_Text[] m_BulletText;

    public Color m_UnusedColor;
    public Color m_UsedColor;

    public BulletUI m_BulletUI;

    private void OnEnable()
    {
        Player_BulletManager.OnUpdateHud += UpdateHUD;
    }
    private void OnDisable()
    {
        Player_BulletManager.OnUpdateHud -= UpdateHUD;
    }
    void UpdateHUD(int[] bulletList)
    {
        Debug.Log("Enter");
        for (int i = 0; i < bulletList.Length; i++)
        {
            if (bulletList[i] >= 0)
            {
                m_BulletImage[i].color = m_UnusedColor;
                m_BulletText[i].text = m_BulletUI.BulletTypeToName(bulletList[i]);
            }
            else
            {
                m_BulletImage[i].color = m_UsedColor;
                m_BulletText[i].text = "";
            }
        }
    }
}
