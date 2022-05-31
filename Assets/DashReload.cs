using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashReload : MonoBehaviour
{
    private Player_Blackboard playerBlackboard;
    private float currentTime = 0;
    [SerializeField] private Image display;
    private bool dashing = false;
    [SerializeField] private float timeAdjust = 0.4f;
    private void OnEnable()
    {
        GameManager.GetManager().GetInputManager().OnStartDashing += StartDashCooldown;
    }
    private void OnDisable()
    {
        GameManager.GetManager().GetInputManager().OnStartDashing -= StartDashCooldown;
    }
    private void Start()
    {
        playerBlackboard = GameManager.GetManager().GetPlayer().GetComponent<Player_Blackboard>();
        currentTime = playerBlackboard.m_DashColdownTime;
    }
    private void StartDashCooldown()
    {
        if (!dashing)
        {
            dashing = true;
            currentTime = 0;
        }
    }

    void Update()
    {
        if (playerBlackboard.m_DashColdownTime+timeAdjust >= currentTime)
        {
            currentTime += Time.deltaTime;
            display.fillAmount = Mathf.Clamp(currentTime / (playerBlackboard.m_DashColdownTime + timeAdjust), 0, 1);
        }
        else
        {
            dashing = false;
        }

    }
}
