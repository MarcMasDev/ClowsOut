using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticket
{
    List<HighFSM> m_Enemies;
    HighFSM m_Enemy1;
    HighFSM m_Enemy2;
    HighFSM m_Enemy3;
    public bool m_IsFull => m_Enemy1 != null && m_Enemy2 != null && m_Enemy3 != null;
    public int m_NumberEnemies => m_Enemies.Count;
   public Ticket(HighFSM enemy1, HighFSM enemy2, HighFSM enemy3)
    {
        m_Enemies = new List<HighFSM>();
        this.m_Enemy1 = enemy1;
        this.m_Enemy2 = enemy2;
        this.m_Enemy3 = enemy3;
        m_Enemies.Add(m_Enemy1);
        m_Enemies.Add(m_Enemy2);
        m_Enemies.Add(m_Enemy3);
        m_Enemy1.transform.GetComponent<HealthSystem>().OnDeath += OnDeathEnemy1;
        m_Enemy2.transform.GetComponent<HealthSystem>().OnDeath += OnDeathEnemy2;
        m_Enemy3.transform.GetComponent<HealthSystem>().OnDeath += OnDeathEnemy3;
    } 
    public Ticket(HighFSM enemy1)
    {
        m_Enemies = new List<HighFSM>();
        this.m_Enemy1 = enemy1;
        m_Enemies.Add(m_Enemy1);
        m_Enemy1.transform.GetComponent<HealthSystem>().OnDeath += OnDeathEnemy1;
    }
    public Ticket() 
    {
        m_Enemies = new List<HighFSM>();
    }
    public bool AddEnemy(HighFSM enemy)
    {
        if(m_Enemy1 == null)
        {
            m_Enemy1 = enemy;
            m_Enemy1.transform.GetComponent<HealthSystem>().OnDeath += OnDeathEnemy1;
            m_Enemies.Add(enemy);
            return true;
        }
        else if(m_Enemy2 == null)
        {
            m_Enemy2 = enemy;
            m_Enemy2.transform.GetComponent<HealthSystem>().OnDeath += OnDeathEnemy2;
            m_Enemies.Add(enemy);
            return true;
        }
        else if(m_Enemy3 == null)
        {
            m_Enemy3 = enemy;
            m_Enemy3.transform.GetComponent<HealthSystem>().OnDeath += OnDeathEnemy3;
            m_Enemies.Add(enemy);
            return true;
        }
        else
        {
            return false;
        }

      

    }
    public void Attack()
    {
        foreach (var enemy in m_Enemies)
        {
            enemy.InvokeAttack();
        }
    }
    public void OnDeathEnemy1()
    {
        m_Enemies.Remove(m_Enemy1);
        m_Enemy1.transform.GetComponent<HealthSystem>().OnDeath -= OnDeathEnemy1;
        m_Enemy1 = null;

    }
    public void OnDeathEnemy2()
    {
        m_Enemies.Remove(m_Enemy2);
        m_Enemy2.transform.GetComponent<HealthSystem>().OnDeath -= OnDeathEnemy2;
        m_Enemy2 = null;

    }
    public void OnDeathEnemy3()
    {
        m_Enemies.Remove(m_Enemy3);
        m_Enemy3.transform.GetComponent<HealthSystem>().OnDeath -= OnDeathEnemy3;
        m_Enemy3 = null;
    }
    public void EnemyOutRange(HighFSM enemy)
    {
        if (m_Enemy1 == enemy)
        {
           
            m_Enemy1.transform.GetComponent<HealthSystem>().OnDeath -= OnDeathEnemy1;
            m_Enemies.Remove(enemy);
            m_Enemy1 = null;
        }
        else if (m_Enemy2 == enemy)
        {
           
            m_Enemy2.transform.GetComponent<HealthSystem>().OnDeath -= OnDeathEnemy2;
            m_Enemies.Remove(enemy);
            m_Enemy2 = null;
        }
        else if (m_Enemy3 == enemy)
        {
           
            m_Enemy3.transform.GetComponent<HealthSystem>().OnDeath -= OnDeathEnemy3;

            m_Enemies.Remove(enemy);
            m_Enemy3 = null;
        }

    }
}
