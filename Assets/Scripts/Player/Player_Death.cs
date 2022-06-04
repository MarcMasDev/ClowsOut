using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Death : MonoBehaviour
{
    HealthSystem m_hp;
    CharacterController m_PlayerController;
    // Start is called before the first frame update
    void Awake()
    {
        m_hp = GetComponent<HealthSystem>();
        m_PlayerController = GetComponent<CharacterController>();
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
        m_PlayerController.enabled = false;
        transform.position = GameManager.GetManager().GetCheckpointsManager().m_lastCheckpoint.position;
        transform.rotation = Quaternion.identity;
        m_PlayerController.enabled = true;

        GameManager.GetManager().GetRestartManager().Restart();
    }
}
