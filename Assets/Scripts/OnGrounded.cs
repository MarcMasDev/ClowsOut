using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGrounded : MonoBehaviour
{
    BlackboardEnemies m_blackboardEnemies;
    private void Start()
    {
        m_blackboardEnemies = GetComponentInParent<BlackboardEnemies>();
    }

    private void OnTriggerEnter(Collider other)
    {
        m_blackboardEnemies.m_IsGrounded = true;
    }
    private void OnTriggerStay(Collider other)
    {
        m_blackboardEnemies.m_IsGrounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        m_blackboardEnemies.m_IsGrounded = false;
    }
}
