using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static ShootSystemManager;

public class BulletMenu : MonoBehaviour
{
    public TMP_Text[] m_SelectableBulletsText;
    public TMP_Text[] m_EquippedBulletsText;
    public Image[] m_EquippedBulletsImage;
    public Image[] m_EquippedBulletsIcons;
    public Sprite m_UnequippedIcon;
    public Color m_UnequippedColor;
    public Color m_EquippedColor;
    public Button Accept;
    
    private bool[] m_MenuEquippedCheck = new bool[3];

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
            m_EquippedBulletsIcons[i].sprite = m_UnequippedIcon;
            m_MenuEquippedCheck[i] = false;
        }
    }
    public void EquipBullet(int n)
    {
        for (int i = 0; i < GameManager.GetManager().GetLevelData().LoadDataPlayerBullets().Length; i++)
        {
            if (m_MenuEquippedCheck[i] == false)
            {
                GameManager.GetManager().GetLevelData().LoadDataPlayerBullets()[i] = (BulletType) n;
                m_EquippedBulletsText[i].text = m_BulletUI.BulletTypeToName(n);
                m_EquippedBulletsImage[i].color = m_EquippedColor;
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
        m_EquippedBulletsText[n].text = "";
        m_EquippedBulletsIcons[n].sprite = m_UnequippedIcon;
        m_EquippedBulletsImage[n].color = m_UnequippedColor;
        m_MenuEquippedCheck[n] = false;
        Accept.interactable = false;
    }
    public void UpdateBulletMenu()
    {
        for (int i = 0; i < GameManager.GetManager().GetLevelData().LoadDataPlayerBullets().Length; i++)
        {
            m_EquippedBulletsText[i].text = m_BulletUI.BulletTypeToName((int)GameManager.GetManager().GetLevelData().LoadDataPlayerBullets()[i]);
            m_EquippedBulletsIcons[i].sprite = m_BulletUI.BulletTypeToSprite((int)GameManager.GetManager().GetLevelData().LoadDataPlayerBullets()[i]);
            m_EquippedBulletsImage[i].color = m_EquippedColor;
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
}
