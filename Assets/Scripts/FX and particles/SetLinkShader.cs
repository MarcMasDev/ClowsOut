using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLinkShader : MonoBehaviour
{
    private Material defaultMaterial;
    [SerializeField] private Material linkMaterial;
    [SerializeField] private SkinnedMeshRenderer enemy_renderer;
    private BlackboardEnemies enemyBlackboard;
    private bool linked = false;

    private void Awake()
    {
        enemyBlackboard = GetComponent<BlackboardEnemies>();
        defaultMaterial = enemy_renderer.material;
    }

    private void Update()
    {
        if (enemyBlackboard.m_IsLinq && !linked)
        {
            linked = true;
            enemy_renderer.material = linkMaterial;
        }
        else if (linked && !enemyBlackboard.m_IsLinq)
        {
            linked = false;
            enemy_renderer.material = defaultMaterial;
        }
    }
}
