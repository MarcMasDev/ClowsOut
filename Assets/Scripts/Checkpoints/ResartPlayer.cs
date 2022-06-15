using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResartPlayer : MonoBehaviour,IRestart
{
    [SerializeField]
    Transform m_player;
    [SerializeField]
    MonoBehaviour[] m_ComponetsToRestart;
    [SerializeField]
    Player_FSM m_playerFSM;
    [SerializeField]
    NavMeshObstacle m_navmesh;
    [SerializeField]
    CharacterController m_characterController;
    [SerializeField]
    HealthSystem m_hp;
    // Start is called before the first frame update
    void Start()
    {
        AddRestartElement();
        m_hp.m_OnDeath += PlayerDies;
    }
    public void PlayerDies(GameObject g)
    {
        print("diePlayer");
        StartCoroutine(RestartPlayer());
    }
    public void AddRestartElement()
    {
        GameManager.GetManager().GetRestartManager().addRestartElement(this,transform);
    }

    public void Restart()
    {
        print("player restart");
        m_characterController.enabled = false;
        m_player.position = GameManager.GetManager().GetCheckpointsManager().m_lastCheckpoint.transform.position;
        m_player.rotation = GameManager.GetManager().GetCheckpointsManager().m_lastCheckpoint.transform.rotation;
        foreach (var component in m_ComponetsToRestart)
        {
            component.enabled = true;
        }
        m_navmesh.enabled = true;
        m_characterController.enabled = true;
        m_playerFSM.Restart();
        //StartCoroutine(RestartPlayer());
    }
    IEnumerator RestartPlayer()
    {

        yield return new WaitForSeconds(0.2f);
        GameManager.GetManager().GetRestartManager().Restart();
    }
}
