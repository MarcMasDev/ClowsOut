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
    Dictionary<HighFSM, Ticket> m_EnemiesInTicket = new Dictionary<HighFSM, Ticket>();
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
        m_elapsedTime += Time.deltaTime;
        if (m_TicketList.Count > 0)
        {
            if (m_elapsedTime > m_TimeBetweenEnemiesAttack)
            {
                m_elapsedTime = 0f;
                m_TicketList[m_index].Attack();
                m_index++;
                if (m_index >=  m_TicketList.Count)
                {
                    m_index = 0;
                }
            }
        }
        
    }
    public void EnemyInRange(HighFSM enemy)
    {
        if (!m_EnemiesInTicket.ContainsKey(enemy))
        {
            AddEnemy(enemy);
        }
    }
    public void EnemyOutRange(HighFSM enemy) 
    {
        if (m_EnemiesInTicket.ContainsKey(enemy))
        {
            m_EnemiesInTicket[enemy].EnemyOutRange(enemy);
            m_EnemiesInTicket.Remove(enemy);
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
            bool l_HaveSpace = false;
            foreach (var ticket in m_TicketList)
            {
                if (!ticket.m_IsFull)
                {
                    m_EnemiesInTicket.Add(enemy, ticket);
                    l_HaveSpace = true;
                    break;
                }
            }
            if (!l_HaveSpace)
            {
                GenerateTicket(enemy);
            }
        }
    }

    public void GenerateTicket(HighFSM enemy)
    {
        Ticket l_ticket = new Ticket(enemy);
        m_TicketList.Add(l_ticket);
        m_EnemiesInTicket.Add(enemy, l_ticket);
    }
}
