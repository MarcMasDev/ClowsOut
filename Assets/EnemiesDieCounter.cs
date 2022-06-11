using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesDieCounter : MonoBehaviour
{
    public int m_DiedEnemies;
    private int m_PreviousCount;

    private void Update()
    {
        
        int l_Count = transform.childCount;
        if (l_Count < m_PreviousCount)
        {
            m_DiedEnemies +=  m_PreviousCount - l_Count;
        }
        m_PreviousCount = l_Count;
    }
}
