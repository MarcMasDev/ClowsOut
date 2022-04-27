using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        
        for (int i = 0; i < m_EnemiesLinqued.Count; i++)
            {
                print(m_EnemiesLinqued[i].gameObject.name + " has been removed");
                m_EnemiesLinqued[i].RemoveLink();
                m_EnemiesLinqued[i]= null;
        }
        m_EnemiesLinqued.Clear();
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
    public bool IceBullet(int interations,float damage, float timeBetweenIteration,float slowSpeed,GameObject enemy)
    {
        BlackboardEnemies l_Enemy = enemy.GetComponent<BlackboardEnemies>();
        if (m_EnemiesLinqued.Find(x => x == l_Enemy))
        { for (int i = 0; i < m_EnemiesLinqued.Count; i++)
            {
                NavMeshAgent l_nav = m_EnemiesLinqued[i].m_nav;
                float l_PreviousSpeed = l_nav.speed;
                l_nav.speed = slowSpeed;
                m_EnemiesLinqued[i].m_IceState.StartStateIce();

                StartCoroutine(
                    TemporalDamageIceBullet(
                        m_EnemiesLinqued[i].m_hp,
                        interations,
                        damage,
                        timeBetweenIteration,
                        l_nav,
                        l_PreviousSpeed
                        ));
            }
            Unsucribe();
            return true;
        }
        else
        {
            return false;
        }
        
    }
    public IEnumerator TemporalDamageIceBullet(HealthSystem hp,int maxIterations,float damage, float timeBetweenIteration, NavMeshAgent l_nav, float prevoiusSpeed)
    {
        int l_CurrIterations = 0;
        HealthSystem l_EnemyHealthSystem = hp;
        while (l_CurrIterations < maxIterations)
        {
            l_EnemyHealthSystem.TakeDamage(damage);
            yield return new WaitForSeconds(timeBetweenIteration);
            l_CurrIterations++;
        }
        l_nav.speed = prevoiusSpeed;
        Debug.Log("Temporal Damage Finished");
    }
}
