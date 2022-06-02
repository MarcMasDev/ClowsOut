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
        transform.position = CheckPoints.m_instance.m_lastCheckpoint.position;
        GameManager.GetManager().GetRestartManager().Restart();
        GameManager.GetManager().GetCanvasManager().End(false);
    }
}
