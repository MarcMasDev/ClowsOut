using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashReload : MonoBehaviour
{
    private Player_Blackboard playerBlackboard;
    private Player_FSM playerFSM;
    [SerializeField] private Image display;
    private void Start()
    {
        playerBlackboard = GameManager.GetManager().GetPlayer().GetComponent<Player_Blackboard>();
        playerFSM = GameManager.GetManager().GetPlayer().GetComponent<Player_FSM>();
    }

    void Update()
    {
        display.fillAmount = Mathf.Clamp(playerFSM.m_DashColdownTimer / (playerBlackboard.m_DashColdownTime), 0, 1);
    }
}
