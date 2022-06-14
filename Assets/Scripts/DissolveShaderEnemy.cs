using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveShaderEnemy : MonoBehaviour
{
    [SerializeField] private float speed = 0.05f;
    [SerializeField] private float max = 2f;
    [SerializeField] private Material enemymat;
    [SerializeField] private Material gunmat;
    [SerializeField] private SkinnedMeshRenderer m_SkinnedMeshRenderer;
    [SerializeField] private MeshRenderer[] m_Renderers;
    [SerializeField] private bool m_Dissolve;
    private float time = 0;
    [SerializeField] private GameObject m_Enemy;
    private void Update()
    {
        if (m_Dissolve)
        {
            time += Time.deltaTime;
            m_SkinnedMeshRenderer.material = enemymat;
            if (time * speed < max)
            {
                m_SkinnedMeshRenderer.material.SetFloat("_Dissapear_amount", time * speed);
                foreach (var item in m_Renderers)
                {
                    item.material.SetFloat("_Dissapear_amount", time * speed);
                }
            }
            else
            {
                m_Dissolve = false;
                m_Enemy.SetActive(false);
                Destroy(m_Enemy);
            }
        }
    }
    public void Dissolve()
    {
        m_SkinnedMeshRenderer.material = enemymat;
        foreach (var item in m_Renderers)
        {
            item.material = gunmat;
        }
        m_Dissolve = true;
    }
}
