using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinqSystem : MonoBehaviour,IRestart
{
    public  static LinqSystem m_Instance;
    List<BlackboardEnemies> m_EnemiesLinqued = new List<BlackboardEnemies>();
    bool m_BlockList = false;

 private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            m_BlockList = false;
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
    public List<BlackboardEnemies> GetLinkedEnemiesForApply(GameObject enemy)
    {
        BlackboardEnemies l_Enemy = enemy.GetComponent<BlackboardEnemies>();
        if (m_EnemiesLinqued.Find(x => x == l_Enemy))
        {
            List<BlackboardEnemies> l_EnemiesLinqued = m_EnemiesLinqued;
            Unsucribe();
            return l_EnemiesLinqued;
        }
        else
        {
            return null;
        }
            
    }
    public void ApplyDamageToMarkEnemies(float damage, GameObject enemy)
    {
        Debug.Log("daño enemigos marcados");
        BlackboardEnemies l_Enemy = enemy.GetComponent<BlackboardEnemies>();
        if (m_EnemiesLinqued.Find(x=> x == l_Enemy))
        {
            for (int i = 0; i < m_EnemiesLinqued.Count; i++)
            {
                print(m_EnemiesLinqued[i].gameObject.name + " reciveDamage");
                m_EnemiesLinqued[i].m_hp.TakeDamage(damage);
            }
            Unsucribe();
        }
    }
    public void Unsucribe()
    {
        if (!m_BlockList)
        {
            m_BlockList = true;
            for (int i = 0; i < m_EnemiesLinqued.Count; i++)
            {
                m_EnemiesLinqued[i].RemoveLink();
            }
            m_EnemiesLinqued = new List<BlackboardEnemies>();
            m_BlockList = false;
        }
        
    }
    public void AddLinqued(BlackboardEnemies m_enemy)
    {
       
        if(!m_EnemiesLinqued.Find(x => x.gameObject == m_enemy.gameObject))
        {
            print(m_enemy.gameObject.name + " added");
            m_EnemiesLinqued.Add(m_enemy);
        }
    }
    public void Removed(BlackboardEnemies m_enemy)
    {
        print(m_enemy.gameObject.name + " Removed");
        m_EnemiesLinqued.Remove(m_enemy);
    }

    public void AddRestartElement()
    {
        RestartElements.m_Instance.addRestartElement(this);
    }

    public void Restart()
    {
        Unsucribe();
    }
}
