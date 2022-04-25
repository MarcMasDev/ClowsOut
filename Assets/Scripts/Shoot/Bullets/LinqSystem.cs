using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinqSystem : MonoBehaviour
{
    LinqSystem m_Instance;
    List<GameObject> m_EnemiesLinqued = new List<GameObject>();
    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            GameObject.Destroy(this);
        }
    }


    public void AddLinqued(GameObject m_enemy)
    {
        m_EnemiesLinqued.Add(m_enemy);

    }  
    public void AddRemoved(GameObject m_enemy)
    {
        m_EnemiesLinqued.Add(m_enemy);

    }
}
