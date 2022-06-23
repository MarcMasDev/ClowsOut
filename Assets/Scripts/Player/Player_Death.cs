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
        if (m_PlayerBlackboard.m_Death)
        {
            if (m_DeathTimer >= m_PlayerBlackboard.m_DeathTime)
            {
                m_PlayerBlackboard.m_Death = false;
                Debug.Log("Here");
                m_PlayerController.enabled = false;
                transform.position = GameManager.GetManager().GetCheckpointsManager().m_lastCheckpoint.position;
                transform.rotation = GameManager.GetManager().GetCheckpointsManager().m_lastCheckpoint.rotation;
                CameraReset();
                m_PlayerController.enabled = true;
                GameManager.GetManager().GetRestartManager().Restart();
                m_DeathTimer = 0f;
                m_OnReviveS?.Invoke();
                
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
        m_PlayerBlackboard.m_Death = true;
    }
    IEnumerator Try()
    {
        Debug.Log("Wait");
        yield return new WaitForSeconds(5f);
        OnDeath(null);
        yield return null;
    }
    void CameraReset()
    {
        m_PlayerBlackboard.m_MediumCamera.enabled = false;
        m_PlayerBlackboard.m_AimCamera.enabled = false;
        m_PlayerBlackboard.m_CinemachineBrain.enabled = false;

        Vector3 l_pos = GameManager.GetManager().GetCheckpointsManager().m_lastCheckpoint.position;
        Quaternion l_root= GameManager.GetManager().GetCheckpointsManager().m_lastCheckpoint.rotation;
        m_PlayerBlackboard.m_MediumCamera.transform.rotation = l_root;
        m_PlayerBlackboard.m_AimCamera.transform.rotation = l_root;
        m_PlayerBlackboard.m_CinemachineBrain.transform.rotation = l_root;
        m_PlayerBlackboard.m_MediumCamera.ForceCameraPosition(l_pos, l_root);
        m_PlayerBlackboard.m_AimCamera.ForceCameraPosition(l_pos, l_root);
        m_PlayerBlackboard.m_MediumCamera.enabled = true;
        m_PlayerBlackboard.m_AimCamera.enabled = true;
        m_PlayerBlackboard.m_CinemachineBrain.enabled = true;
    }
}
