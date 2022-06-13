using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Death : MonoBehaviour
{
    HealthSystem m_hp;
    CharacterController m_PlayerController;
    Player_Blackboard m_PlayerBlackboard;
    public static Action m_OnReviveS;
    public static Action m_OnDeathS;
    bool m_Death;
    float m_DeathTimer;
    // Start is called before the first frame update
    void Awake()
    {
        m_hp = GetComponent<HealthSystem>();
        m_PlayerBlackboard = GetComponent<Player_Blackboard>();
        m_PlayerController = GetComponent<CharacterController>();
        //StartCoroutine(Try());
    }
    private void Update()
    {
        if (m_Death)
        {
            if (m_DeathTimer >= m_PlayerBlackboard.m_DeathTime)
            {
                Debug.Log("Here");
                transform.position = GameManager.GetManager().GetCheckpointsManager().m_lastCheckpoint.position;
                transform.rotation = Quaternion.identity;
                m_PlayerController.enabled = true;
                GameManager.GetManager().GetRestartManager().Restart();
                m_OnReviveS?.Invoke();
                m_Death = false;
            }
            m_DeathTimer += Time.deltaTime;
        }
    }
    private void OnEnable()
    {
        m_hp.m_OnDeath += OnDeath;
    }
    private void OnDisable()
    {
        m_hp.m_OnDeath -= OnDeath;
    }
    public void OnDeath(GameObject g)
    {
        Debug.Log("DEATH");
        m_OnDeathS?.Invoke();
        m_PlayerController.enabled = false;
        m_PlayerBlackboard.m_SetRagdollPlayer.Die();
        m_DeathTimer = 0;
        m_Death = true;
    }
    IEnumerator Try()
    {
        Debug.Log("Wait");
        yield return new WaitForSeconds(5f);
        OnDeath(null);
        yield return null;
    }
}
