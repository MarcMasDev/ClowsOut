using UnityEngine;
using UnityEngine.UI;

public class BulletHUDFinal : MonoBehaviour
{
    public RectTransform m_Drum;
    public Image[] m_BulletHUDImages;
    public Color m_UnusedColor;
    public Color m_UsedColor;
    public BulletUI m_BulletUI;
    public float m_DrumVelocity;

    private int m_CurrentIndex;
    private float m_CurrentRotation;
    private float m_TargetRotation;
    private float m_RotationTime;
    private float m_RotationTimer;
    private void OnEnable()
    {
        Player_BulletManager.OnShoot += Shoot;
        Player_BulletManager.OnRotateClockwise += RotateClockwise;
        Player_BulletManager.OnRotateCounterclockwise += RotateCounterclockwise;
        Player_BulletManager.OnChangeBullets += ChangeBullets;
    }
    private void OnDisable()
    {
        Player_BulletManager.OnShoot -= Shoot;
        Player_BulletManager.OnRotateClockwise -= RotateClockwise;
        Player_BulletManager.OnRotateCounterclockwise -= RotateCounterclockwise;
        Player_BulletManager.OnChangeBullets -= ChangeBullets;
    }
    void Update()
    {
        for (int i = 0; i < m_BulletHUDImages.Length; i++)
        {
            m_BulletHUDImages[i].rectTransform.eulerAngles = Vector3.zero;
        }
        if (!ManagerUI.m_BulletHUDActualized)
        {
            if (Mathf.Abs(m_TargetRotation - m_CurrentRotation) <= 0.1)
            {
                m_CurrentRotation = m_TargetRotation;
                if (m_CurrentRotation < 0)
                {
                    m_CurrentRotation += 360;
                    m_TargetRotation = m_CurrentRotation;
                }
                else if (m_CurrentRotation >= 360)
                {
                    m_CurrentRotation -= 360;
                    m_TargetRotation = m_CurrentRotation;
                }
                ManagerUI.m_BulletHUDActualized = true;
            }
            else
            {
                m_CurrentRotation = Mathf.Lerp(m_CurrentRotation, m_TargetRotation, m_RotationTimer / m_RotationTime);
                m_Drum.eulerAngles = new Vector3(0, 0, m_CurrentRotation);
                m_RotationTimer += Time.deltaTime;
            }
        }
    }
    void Shoot()
    {
        m_BulletHUDImages[m_CurrentIndex].color = m_UsedColor;
    }
    void ChangeBullets(int[] bulletList)
    {
        for (int i = 0; i < bulletList.Length; i++)
        {
            if (bulletList[i] >= 0)
            {
                m_BulletHUDImages[i].sprite = m_BulletUI.BulletTypeToSprite(bulletList[i]);
                m_BulletHUDImages[i].color = m_UnusedColor;
            }
        }
        m_TargetRotation = 1800;
        m_RotationTime = Mathf.Abs(m_TargetRotation - m_CurrentRotation) / m_DrumVelocity;
        m_RotationTimer = 0;
        m_CurrentIndex = 0;
    }
    void RotateClockwise()
    {
        m_CurrentIndex = (m_CurrentIndex + 1) % 3;
        m_TargetRotation += 120f;
        m_RotationTime = Mathf.Abs(m_TargetRotation - m_CurrentRotation) / m_DrumVelocity;
        m_RotationTimer = 0;
    }
    void RotateCounterclockwise()
    {
        m_CurrentIndex -= 1;
        if (m_CurrentIndex < 0)
        {
            m_CurrentIndex = 2;
        }
        m_TargetRotation -= 120f;
        m_RotationTime = Mathf.Abs(m_TargetRotation - m_CurrentRotation) / m_DrumVelocity;
        m_RotationTimer = 0;
    }
}
