using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveShaderEnemy : MonoBehaviour
{
    [SerializeField] private float speed = 0.05f;
    [SerializeField] private float max = 2f;
    [SerializeField] private Material mat;
    [SerializeField] private SkinnedMeshRenderer m_SkinnedMeshRenderer;
    private float time = 0;
    [SerializeField] private GameObject m_Enemy;
    public void Dissolve()
    {
        m_SkinnedMeshRenderer.material = mat;
        StartCoroutine(ExampleCoroutine());
    }
    IEnumerator ExampleCoroutine()
    {
        time += Time.deltaTime;
        m_SkinnedMeshRenderer.material.SetFloat("_Dissapear_amount", time * speed);
        yield return new WaitForSeconds(0.1f);
        if (m_SkinnedMeshRenderer.material.GetFloat("_Dissapear_amount") < max)
        {
            StartCoroutine(ExampleCoroutine());
        }
        else
        {
            m_SkinnedMeshRenderer.material.SetFloat("_Dissapear_amount", 0);
            time = 0;
            Destroy(m_Enemy);
        }
    }
}
