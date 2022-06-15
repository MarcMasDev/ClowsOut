using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemyUI : MonoBehaviour
{
    BlackboardEnemies m_BlackboardEnemies;
    public GameObject m_EnemyLifeBar;
    HealthSystem m_hp;
    LifeBarEnemyPosition m_LifeBar;
    public Transform m_UIAnchor;
    float m_HeighEnemy = 1.6f;
    // Start is called before the first frame update
    void Start()
    {
        m_hp = GetComponent<HealthSystem>();
        m_hp.m_OnHit += ShowLifeAfterDamage;
        m_BlackboardEnemies = GetComponent<BlackboardEnemies>();
        CreateLifeBar();
        
    }
    private void LateUpdate()
    {
        Debug.Log(m_UIAnchor);
        Debug.Log(m_BlackboardEnemies.m_IsLinq);
        m_LifeBar.SetLifeBarEnemy(m_UIAnchor.position, m_BlackboardEnemies.m_IsLinq);
        if (!IsPlayerWatchingMe())
        {
            m_LifeBar.DontShow();
            m_LifeBar.HideLinqIcon();
        }
        if (!m_BlackboardEnemies.m_IsLinq)
        {
            m_LifeBar.HideLinqIcon();
        }
            
        
    }
    void CreateLifeBar()
    {
        HealthBarEnemy l_HealthBar = Instantiate(m_EnemyLifeBar.gameObject, GameManager.GetManager().GetCanvasManager().m_LifeBarParent).GetComponent<HealthBarEnemy>();
        m_LifeBar = l_HealthBar.gameObject.GetComponent<LifeBarEnemyPosition>();
        l_HealthBar.m_hp = m_hp;
        l_HealthBar.gameObject.SetActive(true);
        l_HealthBar.Init();
    }
    bool IsPlayerWatchingMe()
    {
        Vector3 l_PlayerPosition = m_BlackboardEnemies.m_Player.position + Vector3.up * m_HeighEnemy;
        Vector3 l_EyesEnemyPosition = transform.position + Vector3.up * m_HeighEnemy;
        Vector3 l_Direction = l_EyesEnemyPosition - l_PlayerPosition;
        float l_DistanceBetwenObjects = l_Direction.magnitude;
        l_Direction /= l_DistanceBetwenObjects;
        Ray l_ray = new Ray(l_PlayerPosition, l_Direction);
        Vector3 l_forward = transform.forward;
        l_forward.y = 0;
        l_forward.Normalize();
        l_Direction.y = 0;
        l_Direction.Normalize();
        if (!Physics.Raycast(l_ray, l_DistanceBetwenObjects, m_BlackboardEnemies.m_CollisionLayerMask.value))
        {
            //Debug.DrawLine(l_EyesEnemyPosition, l_PlayerPosition, Color.red);
            return true;
        }
        else { return false; }
    }
    public void ShowLifeAfterDamage(float d)
    {
        m_LifeBar.OnTakeDamage();
    }
}
