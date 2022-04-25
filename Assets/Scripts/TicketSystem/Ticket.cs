using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticket
{
    List<HighFSM> m_Enemies;
    private int m_TicketLimit = 3;
    public int m_NumberEnemies => m_Enemies.Count;
    public bool m_IsFull => !m_Enemies.Contains(null);

   public Ticket(List<HighFSM> enemyList)
    {
        m_Enemies = new List<HighFSM>(enemyList);
        SuscribeEnemyOnDeath(m_Enemies);
    } 
    public Ticket(HighFSM enemy)
    {
        m_Enemies = new List<HighFSM>();
        m_Enemies.Add(enemy);
        m_Enemies.Add(null);
        m_Enemies.Add(null);
        SuscribeEnemyOnDeath(enemy);
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
                    m_Enemies.Insert(i, enemy);
                    SuscribeEnemyOnDeath(enemy);
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
                    UnsubscribeEnemyOnDeath(enemy);
                    return;
                }
            }
        }
    }
    public void Attack()
    {
        Debug.Log("ticket attack");
        foreach (var enemy in m_Enemies)
        {
<<<<<<< HEAD
            enemy.InvokeAttack();
            Debug.Log("ticket attack enemy " + enemy.gameObject.name );
=======
            if (enemy != null)
            {
                enemy.InvokeAttack();
            }
>>>>>>> Ricard
        }
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
            enemyList[i].transform.GetComponent<HealthSystem>().OnDeath += OnDeathEnemy;
        }
    }
    public void SuscribeEnemyOnDeath(HighFSM enemy)
    {
        enemy.transform.GetComponent<HealthSystem>().OnDeath += OnDeathEnemy;
    }
    public void UnsubscribeEnemyOnDeath(HighFSM enemy)
    {
        enemy.transform.GetComponent<HealthSystem>().OnDeath -= OnDeathEnemy;
    }
}
