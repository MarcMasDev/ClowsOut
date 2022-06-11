using System.Collections;
using UnityEngine;

public class DissolveShaderPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 0.05f;
    [SerializeField] private float max = 2f;
    [SerializeField] private Material playermat;
    [SerializeField] private Material gunmat;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private MeshRenderer[] renderers;
    private float time = 0;
    [SerializeField] private GameObject m_Enemy;
    public void Dissolve()
    {
        skinnedMeshRenderer.material = playermat;
        foreach (var item in renderers)
        {
            item.material = gunmat;
        }
        StartCoroutine(DissolveCoroutine());
    }
    public void Appear()
    {
        skinnedMeshRenderer.material = playermat;
        foreach (var item in renderers)
        {
            item.material = gunmat;
        }
        skinnedMeshRenderer.material.SetFloat("_Dissapear_amount", max);
        foreach (var item in renderers)
        {
            item.material.SetFloat("_Dissapear_amount", max);
        }
        skinnedMeshRenderer.enabled = true;
        foreach (var item in renderers)
        {
            item.enabled = true;
        }
        time = 0;
        StartCoroutine(AppearCoroutine());
    }
    IEnumerator DissolveCoroutine()
    {
        time += Time.deltaTime;
        skinnedMeshRenderer.material.SetFloat("_Dissapear_amount", time * speed);
        foreach (var item in renderers)
        {
            item.material.SetFloat("_Dissapear_amount", time * speed);
        }
        yield return new WaitForSeconds(0.1f);
        if (skinnedMeshRenderer.material.GetFloat("_Dissapear_amount") <= 0.05)
        {
            StartCoroutine(DissolveCoroutine());
        }
        else
        {
            skinnedMeshRenderer.material.SetFloat("_Dissapear_amount", 0);
            foreach (var item in renderers)
            {
                item.material.SetFloat("_Dissapear_amount", 0);
            }
            time = 0;
        }
    }
    IEnumerator AppearCoroutine()
    {

        time += Time.deltaTime;
        skinnedMeshRenderer.material.SetFloat("_Dissapear_amount", max - time * speed);
        foreach (var item in renderers)
        {
            item.material.SetFloat("_Dissapear_amount", max - time * speed);
        }
        yield return new WaitForSeconds(0.1f);
        if (skinnedMeshRenderer.material.GetFloat("_Dissapear_amount") < max)
        {
            StartCoroutine(AppearCoroutine());
        }
        else
        {
            skinnedMeshRenderer.material.SetFloat("_Dissapear_amount", max);
            foreach (var item in renderers)
            {
                item.material.SetFloat("_Dissapear_amount", time * speed);
            }
            skinnedMeshRenderer.enabled = false;
            foreach (var item in renderers)
            {
                item.enabled = false;
            }
            time = 0;
        }
    }
}
