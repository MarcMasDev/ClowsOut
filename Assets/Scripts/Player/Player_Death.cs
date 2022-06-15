using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Death : MonoBehaviour
{
    HealthSystem m_hp;
    CharacterController m_PlayerController;
    Player_Blackboard m_PlayerBlackboard;
    public Action m_OnReviveS;
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
                m_Death = false;
                Debug.Log("Here");
                m_PlayerController.enabled = false;
                transform.position = GameManager.GetManager().GetCheckpointsManager().m_lastCheckpoint.position;
                transform.rotation = GameManager.GetManager().GetCheckpointsManager().m_lastCheckpoint.rotation;
                m_PlayerController.enabled = true;
                Debug.Log("Here1");
                GameManager.GetManager().GetRestartManager().Restart();
                Debug.Log("Here2");
                m_DeathTimer = 0f;
                m_OnReviveS?.Invoke();
                Debug.Log("Here3");
                print("Revive");    
                
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
