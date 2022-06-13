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
    private float time = 0;
    [SerializeField] private GameObject m_Enemy;
    public void Dissolve()
    {
        m_SkinnedMeshRenderer.material = enemymat;
        foreach (var item in m_Renderers)
        {
            item.material = gunmat;
        }
        StartCoroutine(ExampleCoroutine());
    }
    IEnumerator ExampleCoroutine()
    {
        time += Time.deltaTime;
        m_SkinnedMeshRenderer.material = enemymat;
        m_SkinnedMeshRenderer.material.SetFloat("_Dissapear_amount", time * speed);
        foreach (var item in m_Renderers)
        {
            item.material.SetFloat("_Dissapear_amount", time * speed);
        }
        yield return new WaitForSeconds(0.1f);
        if (m_SkinnedMeshRenderer.material.GetFloat("_Dissapear_amount") < max)
        {
            StartCoroutine(ExampleCoroutine());
        }
        else
        {
            m_SkinnedMeshRenderer.material.SetFloat("_Dissapear_amount", max);
            foreach (var item in m_Renderers)
            {
                item.material.SetFloat("_Dissapear_amount", max);
            }
            time = 0;
            Destroy(m_Enemy);
        }
    }
}
