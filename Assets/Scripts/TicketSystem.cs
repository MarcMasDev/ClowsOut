using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketSystem : MonoBehaviour
{
    public static TicketSystem m_Instance = null;
    public Action OnEnemyInRange;
    public List<HighFSM> m_EnemiesInRangeList;
    public float m_TimeBetweenEnemiesAttack = 1f;
    float m_elapsedTime = 0f;
    bool m_RestartList = true;
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
        if(m_RestartList && m_EnemiesInRangeList.Count >= 1)
        {
            StartCoroutine(IEAttack());
        }
    }
    public void EnemyInRange(HighFSM enemy)
    {
        //TODO Revisar tipo de enemigo ya en la lista y mirar si se agrega este o no
        m_EnemiesInRangeList.Add(enemy);
    }
    public void EnemyOutRange(HighFSM enemy) //TODO el onDeath debe llamar a esto también
    {
        m_EnemiesInRangeList.Remove(enemy);
    }
    IEnumerator IEAttack()
    {
        m_RestartList = false;
        foreach (var enemy in m_EnemiesInRangeList)
        {
            enemy.InvokeAttack();
            yield return new WaitForSeconds(m_TimeBetweenEnemiesAttack);
        }
        m_RestartList = true;

    }
}
