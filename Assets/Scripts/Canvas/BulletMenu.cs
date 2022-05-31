using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static ShootSystemManager;
using System.Collections;

public class BulletMenu : MonoBehaviour
{
    public Image[] m_EquippedBulletsIcons;
    public Image[] m_SelectableBullets;

    public Sprite m_UnequippedIcon;
    public Color m_UnequippedColor;
    public Color m_EquippedColor;
    public Button Accept;
    public GameObject m_BaseBullets;
    
    private bool[] m_MenuEquippedCheck = new bool[3];

    public BulletUI m_BulletUI;
    public float timerClock=0.5f;
    bool m_Clocking;

    void Start()
    {
        InitBulletMenu();
    }
    private void InitBulletMenu()
    {
        for (int i = 0; i < m_EquippedBulletsIcons.Length; i++)
        {
            m_EquippedBulletsIcons[i].color = m_UnequippedColor;
            m_EquippedBulletsIcons[i].sprite = m_UnequippedIcon;
            m_MenuEquippedCheck[i] = false;
        }
    }
    public void EquipBullet(int n)
    {
        if (m_Clocking || Accept.IsInteractable())
            return;

     //   StartCoroutine(ClockBullets());
        for (int i = 0; i < GameManager.GetManager().GetLevelData().LoadDataPlayerBullets().Length; i++)
        {
            if (m_MenuEquippedCheck[i] == false)
            {
                GameManager.GetManager().GetLevelData().LoadDataPlayerBullets()[i] = (BulletType) n;
                //m_EquippedBulletsText[i].text = m_BulletUI.BulletTypeToName(n);
                m_EquippedBulletsIcons[i].color = m_EquippedColor;
                m_EquippedBulletsIcons[i].sprite = m_BulletUI.BulletTypeToSprite(n);
                m_MenuEquippedCheck[i] = true;
                CheckAccept();
                return;
            }
        }
    }
    public void UnequipBullet(int n)
    {
        GameManager.GetManager().GetLevelData().LoadDataPlayerBullets()[n] = default;
        m_EquippedBulletsIcons[n].sprite = m_UnequippedIcon;
        m_EquippedBulletsIcons[n].color = m_UnequippedColor;
        m_MenuEquippedCheck[n] = false;
        Accept.interactable = false;
    }
    public void UpdateBulletMenu()
    {
        for (int i = 0; i < GameManager.GetManager().GetLevelData().LoadDataPlayerBullets().Length; i++)
        {
            m_EquippedBulletsIcons[i].sprite = m_BulletUI.BulletTypeToSprite((int)GameManager.GetManager().GetLevelData().LoadDataPlayerBullets()[i]);
            m_EquippedBulletsIcons[i].color = m_EquippedColor;
            m_MenuEquippedCheck[i] = true;
        }
        CheckAccept();
    }

   
    //TODO: Clean this
    public void CheckAccept()
    {
        for (int i = 0; i < m_MenuEquippedCheck.Length; i++)
        {
            if (m_MenuEquippedCheck[i] == false)
            {
                Accept.interactable = false;
                GameManager.GetManager().GetPlayerBulletManager().SetBulletList(GameManager.GetManager().GetLevelData().LoadDataPlayerBullets());
                return;
            }
        }
        Accept.interactable = true;
    }


    IEnumerator ClockBullets()
    {
        float t = 0; 
        float rot = m_BaseBullets.transform.localEulerAngles.z;
        float dest =rot+120;
        m_Clocking = true;
        while (t < timerClock)
        {
            t += Time.deltaTime;
            float z = Mathf.Lerp(rot, dest, t / timerClock);
            m_BaseBullets.transform.localEulerAngles = new Vector3(0, 0, z); 
            yield return null;
        }
        m_Clocking = false;
    }
}
