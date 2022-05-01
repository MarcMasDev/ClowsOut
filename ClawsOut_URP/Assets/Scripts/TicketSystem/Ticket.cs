using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticket
{
    private static int ID;
    public int m_ID;
    List<HighFSM> m_Enemies;
    private int m_TicketLimit = 3;
    public int m_NumberEnemies;
    public bool m_IsFull => m_NumberEnemies == m_TicketLimit;
    public bool m_IsEmpty => m_NumberEnemies == 0;

    public Ticket(List<HighFSM> enemyList)
    {
        m_Enemies = new List<HighFSM>(enemyList);
        SuscribeEnemyOnDeath(m_Enemies);
        m_ID = ID;
        ID++;
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            m_NumberEnemies++;
        }
    } 
    public Ticket(HighFSM enemy)
    {
        m_Enemies = new List<HighFSM>();
        m_Enemies.Add(enemy);
        m_Enemies.Add(null);
        m_Enemies.Add(null);
        SuscribeEnemyOnDeath(enemy);
        m_ID = ID;
        ID++;
        m_NumberEnemies++;
    }
    public Ticket() 
    {
        m_Enemies = new List<HighFSM>();
    }
    public void AddEnemy(HighFSM enemy)
    {
        if (!m_IsFull)
        {
            for (int i = 0; i < m_Enemies.Count; i++)
            {
                if (m_Enemies[i] == null)
                {
                    m_Enemies[i] = enemy;
                    SuscribeEnemyOnDeath(enemy);
                    m_NumberEnemies++;
                    return;
                }
            }
        }
    }
    public void RemoveEnemy(HighFSM enemy)
    {
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            if (m_Enemies[i] != null)
            {
                if (m_Enemies[i].m_ID == enemy.m_ID)
                {
                    m_Enemies[i] = null;
                    m_NumberEnemies--;
                    UnsubscribeEnemyOnDeath(enemy);
                    if (m_IsEmpty)
                    {
                        TicketSystem.m_Instance.RemoveTicket(this);
                    }
                    return;
                }
            }
        }
    }
    public void Attack()
    {
        foreach (var enemy in m_Enemies)
        {
            if (enemy != null)
            {
                enemy.InvokeAttack();
            }
        }
    }
    public bool ContainEnemy(HighFSM enemy)
    {
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            if (m_Enemies[i] != null)
            {
                if (m_Enemies[i].m_ID == enemy.m_ID)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void OnDeathEnemy(GameObject gameObject)
    {
        HighFSM l_Enemy = gameObject.GetComponent<HighFSM>();
        if (l_Enemy)
        {
            RemoveEnemy(l_Enemy);
        }
    }
    public void SuscribeEnemyOnDeath(List<HighFSM> enemyList)
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].transform.GetComponent<HealthSystem>().m_OnDeath += OnDeathEnemy;
        }
    }
    public void SuscribeEnemyOnDeath(HighFSM enemy)
    {
        enemy.transform.GetComponent<HealthSystem>().m_OnDeath += OnDeathEnemy;
    }
    public void UnsubscribeEnemyOnDeath(HighFSM enemy)
    {
        enemy.transform.GetComponent<HealthSystem>().m_OnDeath -= OnDeathEnemy;
    }
}
