using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackboardEnemies : MonoBehaviour
{
    public Transform m_Player;
    public float m_RangeAttack = 15f;
    public float m_IdealRangeAttack = 10f;
    public float m_RangeToNear = 5f;
    public float m_MoveDistanceAfterAttack = 8f;
    public bool m_FinishAttack = false;
    public float m_distanceToPlayer;
    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_distanceToPlayer = Vector3.Distance(m_Player.position, transform.position);
    }
}
