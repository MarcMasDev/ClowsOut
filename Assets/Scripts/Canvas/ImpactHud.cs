using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImpactHud : MonoBehaviour
{
    [SerializeField]
    HealthSystem m_hp;
    CanvasGroup m_Image;
    [SerializeField]
    float m_SeedIncrementAlpha = 1.5f;
    [SerializeField]
    float m_SeedDecrementAlpha = 4f;
    bool m_Damage = false;
    [SerializeField]
    float m_MaxAlpha = 0.7f;
    float m_timer = 0f;
    [SerializeField]
    float m_timeAtMaximum = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        m_Image = GetComponent<CanvasGroup>();
        //m_hp = GameManager.GetManager().GetPlayer().GetComponent<HealthSystem>();
        m_hp.m_OnHit += OnHit;
    }
    private void Update()
    {
        if (m_Damage)
        {
            if (m_Image.alpha <= m_MaxAlpha)
            {
                m_Image.alpha += m_SeedIncrementAlpha * Time.deltaTime;
            }
            else
            {
                m_timer += Time.deltaTime;
                if(m_timer>= m_timeAtMaximum)
                {
                    m_timer = 0f;
                    m_Damage = false;
                }
            }
        }
        else
        {
            m_Image.alpha -= m_SeedDecrementAlpha * Time.deltaTime;
        }
    }
    public void OnHit(float f)
    {
        m_Damage = true;
    }
}
