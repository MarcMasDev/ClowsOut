using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashReload : MonoBehaviour
{
    private Player_FSM playerFSM;
    private Player_Blackboard playerBlackboard;
    private float currentTime = 0;
    private Image display;
    private void OnEnable()
    {
        if (playerFSM)
            playerFSM.OnStartDashing += StartDashCooldown;
    }
    private void OnDisable()
    {
        if (playerFSM)
            playerFSM.OnStartDashing -= StartDashCooldown;
    }
    private void Start()
    {
        playerFSM = GameManager.GetManager().GetPlayer().GetComponent<Player_FSM>();
        playerBlackboard = GameManager.GetManager().GetPlayer().GetComponent<Player_Blackboard>();
        display = GetComponent<Image>();
    }
    private void StartDashCooldown()
    {
        print("DASH");
        currentTime = 0;
    }

    void Update()
    {
        print(playerBlackboard.m_DashColdownTime <= currentTime);
        if (playerBlackboard.m_DashColdownTime <= currentTime)
        {
            currentTime += Time.deltaTime;
            display.fillAmount = Mathf.Clamp(currentTime / playerBlackboard.m_DashColdownTime, 0, 1);
        }

    }
}
