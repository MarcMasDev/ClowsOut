using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLinkShader : MonoBehaviour
{
    [SerializeField] private GameObject defaultRenderer;
    [SerializeField] private GameObject linkRenderer;
    private BlackboardEnemies enemyBlackboard;

    private void Awake()
    {
        enemyBlackboard = GetComponent<BlackboardEnemies>();
        defaultRenderer.SetActive(true);
        linkRenderer.SetActive(false);
    }

    private void Update()
    {
        if (enemyBlackboard.m_IsLinq && defaultRenderer.activeSelf)
        {
            defaultRenderer.SetActive(false);
            linkRenderer.SetActive(true);
        }
        else if (!defaultRenderer.activeSelf && !enemyBlackboard.m_IsLinq)
        {
            defaultRenderer.SetActive(true);
            linkRenderer.SetActive(false);
        }
    }
}
