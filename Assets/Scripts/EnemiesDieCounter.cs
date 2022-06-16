using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesDieCounter : MonoBehaviour,IRestart
{
    public int m_DeathEnemies;
    private int m_PreviousCount;
    private void Start()
    {
        AddRestartElement();
    }
    public void AddRestartElement()
    {
        GameManager.GetManager().GetRestartManager().addRestartElement(this,transform);
    }

    public void Restart()
    {
        m_DeathEnemies = 0;
        m_PreviousCount = 0;
    }

    private void LateUpdate()
    {
        
        int l_Count = transform.childCount;
        if (l_Count < m_PreviousCount)
        {
            m_DeathEnemies +=  m_PreviousCount - l_Count;
        }
        m_PreviousCount = l_Count;
    }
}
