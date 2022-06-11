using System.Collections;
using UnityEngine;

public class DissolveShaderPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 0.05f;
    [SerializeField] private float max = 2f;
    [SerializeField] private Material playermat;
    [SerializeField] private Material gunmat;
    [SerializeField] private Material eyemat;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private MeshRenderer gunrender;
    [SerializeField] private MeshRenderer gunrender1;
    [SerializeField] private MeshRenderer eyerender;
    [SerializeField] private MeshRenderer eyerender1;
    private float time = 0;
    public void Dissolve()
    {
        skinnedMeshRenderer.material = playermat;
        skinnedMeshRenderer.enabled = true;
        gunrender.enabled = true;
        gunrender1.enabled = true;
        eyerender.enabled = true;
        eyerender1.enabled = true;
        //gunrender.material = gunmat;
        //eyerender.material = eyemat;
        StopCoroutine(DissolveCoroutine());
        StartCoroutine(DissolveCoroutine());
    }
    IEnumerator DissolveCoroutine()
    {
        time += Time.deltaTime;
        skinnedMeshRenderer.material.SetFloat("_Dissapear_amount", time * speed);
        //gunrender.material.SetFloat("_Dissapear_amount", time * speed);
        //gunrender1.material.SetFloat("_Dissapear_amount", time * speed);
        //eyerender.material.SetFloat("_Dissapear_amount", time * speed);
        //eyerender1.material.SetFloat("_Dissapear_amount", time * speed);
        yield return new WaitForSeconds(0.1f);
        if (skinnedMeshRenderer.material.GetFloat("_Dissapear_amount") < max)
        {
            StartCoroutine(DissolveCoroutine());
        }
        else
        {
            skinnedMeshRenderer.material.SetFloat("_Dissapear_amount", 0);
            //gunrender.material.SetFloat("_Dissapear_amount", max);
            //gunrender1.material.SetFloat("_Dissapear_amount", max);
            //eyerender.material.SetFloat("_Dissapear_amount", max);
            //eyerender1.material.SetFloat("_Dissapear_amount", max);
            skinnedMeshRenderer.enabled = false;
            gunrender.enabled = false;
            gunrender1.enabled = false;
            eyerender.enabled = false;
            eyerender1.enabled = false;
            time = 0;
        }
    }
}
