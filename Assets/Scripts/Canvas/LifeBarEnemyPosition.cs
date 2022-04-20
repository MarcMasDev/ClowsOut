using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarEnemyPosition : MonoBehaviour
{
    RectTransform m_LifeBar;
    Camera m_Camera;
    private void Start()
    {
        m_LifeBar = gameObject.GetComponent<RectTransform>();
        m_Camera = Camera.main;
    }
    public void SetLifeBarEnemy(Vector3 WorldPosition)
    {
        Vector3 l_ViewportPoint = m_Camera.WorldToScreenPoint(WorldPosition);
        m_LifeBar.transform.position = l_ViewportPoint;
        
        if (l_ViewportPoint.z > 0.0f)
        {
            m_LifeBar.gameObject.SetActive(true);
        }
        else
        {
            m_LifeBar.gameObject.SetActive(false);
        }
    }
    public void DontShow()//Esta funcion se llama siempre que el player no pueda ver al dron
    {
        m_LifeBar.gameObject.SetActive(false);
    }
}
