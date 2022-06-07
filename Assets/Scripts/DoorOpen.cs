using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] int m_MinDeadEnemies;
    [SerializeField] float m_OpenDistance;
    [SerializeField] EnemiesDieCounter m_Enemies;
    [SerializeField] Animator m_Animator;
    private Transform m_PlayerTransform;

    void Start()
    {
        m_PlayerTransform = GameManager.GetManager().GetPlayer().transform;
    }

    void Update()
    {
        //Debug.Log(m_Animator.name + " " + m_Enemies.m_DiedEnemies);
        if (m_Enemies.m_DiedEnemies >= m_MinDeadEnemies)
        {//open, when pass close
            //GameManager.GetManager().GetLastEnemyDeath().position;

            //m_Animator.SetBool("Open", true);

            //if (Vector3.Distance(m_PlayerTransform.position, transform.position) <= m_OpenDistance)
            //{

            //}
            //else
            //{
            //    m_Animator.SetBool("Open", false);
            //}
        }
    }
}
