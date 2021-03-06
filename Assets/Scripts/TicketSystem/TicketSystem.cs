using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketSystem : MonoBehaviour,IRestart
{
    public static TicketSystem m_Instance = null;
    public Action OnEnemyInRange;
    public float m_TimeBetweenEnemiesAttack = 1f;
    [SerializeField]
    List<Ticket> m_TicketList = new List<Ticket>();
    [SerializeField]
    List<HighFSM> m_EnemyList = new List<HighFSM>();
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
    private void Start()
    {
        AddRestartElement();
    }
    // Update is called once per frame
    void Update()
    {
        m_elapsedTime += Time.deltaTime;
        if (m_TicketList.Count > 0)
        {
            if (m_elapsedTime > m_TimeBetweenEnemiesAttack)
            {
                if (m_index < m_TicketList.Count)
                {
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
        if (!m_EnemyList.Find(x => (x.m_ID == enemy.m_ID)))
        {
            AddEnemy(enemy);
        }
    }
    public void EnemyOutRange(HighFSM enemy) 
    {
        int l_EnemyIndex = FindEnemyIndex(enemy, m_EnemyList);
        if (l_EnemyIndex >= 0)
        {
            for (int i = 0; i < m_TicketList.Count; i++)
            {
                if (m_TicketList[i].ContainEnemy(enemy))
                {

                    m_TicketList[i].RemoveEnemy(enemy);
                }
            }
            m_EnemyList.RemoveAt(l_EnemyIndex);
        }
    }

    internal void RemoveTicket(Ticket ticket)
    {
        for (int i = 0; i < m_TicketList.Count; i++)
        {
            if (m_TicketList[i].m_ID == ticket.m_ID)
            {
                m_TicketList.RemoveAt(i);
            }
        }
    }

    void AddEnemy(HighFSM enemy)
    {
        bool l_isAdded = false;
        foreach (var highFSM in m_EnemyList)
        {
            if(enemy.m_ID == highFSM.m_ID)
            {
                l_isAdded = true;
            }
        }
       if(!l_isAdded)
        {
            if (m_TicketList.Count == 0)
            {
                GenerateTicket(enemy);
                //  m_EnemyList.Add(enemy); 
            }
            else
            {
                foreach (var ticket in m_TicketList)
                {
                    if (!ticket.m_IsFull)
                    {
                        ticket.AddEnemy(enemy);
                        m_EnemyList.Add(enemy);
                        return;
                    }
                }
                GenerateTicket(enemy);
                m_EnemyList.Add(enemy);
            }
        }
        
    }

    public void GenerateTicket(HighFSM enemy)
    {
        Ticket l_ticket = new Ticket(enemy);
        m_EnemyList.Add(enemy);
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

    public void AddRestartElement()
    {
        GameManager.GetManager().GetRestartManager().addRestartElement(this, transform);
    }

    public void Restart()
    {
        m_TicketList = new List<Ticket>();
        m_EnemyList = new List<HighFSM>();
    }
}
