using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketSystem : MonoBehaviour
{
    public static TicketSystem m_Instance = null;
    public Action OnEnemyInRange;
    public float m_TimeBetweenEnemiesAttack = 1f;
    List<Ticket> m_TicketList = new List<Ticket>();
    List<HighFSM> m_EnemiyList = new List<HighFSM>();
    float m_elapsedTime = 0f;
    bool m_RestartList = true;
    int m_index = 0;
    private void Awake()
    {
        if(m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            GameObject.Destroy(this);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //TICKET AND ENEMIES
        m_elapsedTime += Time.deltaTime;
        if (m_TicketList.Count > 0)
        {
            if (m_elapsedTime > m_TimeBetweenEnemiesAttack)
            {
                if (m_index < m_TicketList.Count)
                {
                    //Debug.Log("Index: " + m_index + " Attacking, Index Lenght: " + m_TicketList[m_index].m_NumberEnemies) ;
                    m_elapsedTime = 0f;

                    m_TicketList[m_index].Attack();
                    m_index++;
                }
                else
                {
                    m_index = 0;
                }
            }
        }
        
    }
    public void EnemyInRange(HighFSM enemy)
    {
        if (!m_EnemiyList.Find(x => (x.m_ID == enemy.m_ID)))
        {
            AddEnemy(enemy);
        }
    }
    public void EnemyOutRange(HighFSM enemy) 
    {
        int l_EnemyIndex = FindEnemyIndex(enemy, m_EnemiyList);
        if (l_EnemyIndex > 0)
        {
            m_TicketList[l_EnemyIndex].RemoveEnemy(enemy);
            m_EnemiyList.RemoveAt(l_EnemyIndex);
            m_TicketList.RemoveAt(l_EnemyIndex);
        }
    }

    void AddEnemy(HighFSM enemy)
    {
        if(m_TicketList.Count == 0)
        {
            GenerateTicket(enemy);
        }
        else
        {
            foreach (var ticket in m_TicketList)
            {
                if (!ticket.m_IsFull)
                {
                    ticket.AddEnemy(enemy);
                    m_EnemiyList.Add(enemy);
                    m_TicketList.Add(ticket);
                    return;
                }
            }
            Debug.Log("GENERATE TICKET");
            GenerateTicket(enemy);
        }
    }

    public void GenerateTicket(HighFSM enemy)
    {
        Ticket l_ticket = new Ticket(enemy);
        m_TicketList.Add(l_ticket);
        m_EnemiyList.Add(enemy);
        m_TicketList.Add(l_ticket);
    }

    private int FindEnemyIndex(HighFSM enemy, List<HighFSM> enemyList)
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].m_ID == enemy.m_ID)
            {
                return i;
            }
        }
        return -1;
    }
}
