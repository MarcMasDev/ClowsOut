using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] float m_OpenDistance;
    [SerializeField] GameObject m_Enemies;
    [SerializeField] Animator m_Animator;
    private Transform m_PlayerTransform;

    void Start()
    {
        m_PlayerTransform = GameManager.GetManager().GetPlayer().transform;
    }

    void Update()
    {
        bool l_RoomClear = m_Enemies.transform.childCount == 0;
        if (l_RoomClear)
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
