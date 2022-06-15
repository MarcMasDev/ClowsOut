using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashReload : MonoBehaviour
{
    private Player_Blackboard playerBlackboard;
    private Player_FSM playerFSM;
    [SerializeField] private Slider display;
    private void Start()
    {
        playerBlackboard = GameManager.GetManager().GetPlayer().GetComponent<Player_Blackboard>();
        playerFSM = GameManager.GetManager().GetPlayer().GetComponent<Player_FSM>();
    }

    void Update()
    {
        float l_Amount = (playerFSM.m_DashColdownTimer / (playerBlackboard.m_DashColdownTime)) / (1) * (0.7f - 0.1f) + 0.1f;
        display.value = l_Amount;
    }
}
