using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinqSystem : MonoBehaviour
{
    public  static LinqSystem m_Instance;
    List<BlackboardEnemies> m_EnemiesLinqued = new List<BlackboardEnemies>();
    bool m_BlockList = false;

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
    public List<BlackboardEnemies> GetLinkedEnemiesForApply()
    {
        List<BlackboardEnemies> l_EnemiesLinqued = m_EnemiesLinqued;
        Unsucribe();
        return l_EnemiesLinqued;
    }
    public void AplyDamageToMarkEnemies(float damage)
    {
        foreach (var enemy in m_EnemiesLinqued)
        {
            enemy.m_hp.TakeDamage(damage);
        }
        Unsucribe();
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
        m_EnemiesLinqued.Add(m_enemy);
    }
    public void Removed(BlackboardEnemies m_enemy)
    {
        m_EnemiesLinqued.Remove(m_enemy);
    }
}
