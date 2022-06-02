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
        Debug.Log(Vector3.Distance(m_PlayerTransform.position, transform.position));
        if (m_Enemies.m_DiedEnemies >= m_MinDeadEnemies)
        {
            if (Vector3.Distance(m_PlayerTransform.position, transform.position) <= m_OpenDistance)
            {
                m_Animator.SetBool("Open", true);
            }
            else
            {
                m_Animator.SetBool("Open", false);
            }
        }
    }
}
