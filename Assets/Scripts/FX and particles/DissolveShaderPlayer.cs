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
    public Animator animator;
    private float time = 0;
    Material m_oldMatPlayer;
    Material m_oldMatgun;
    Material m_oldMateye;
    [SerializeField]
    Player_Death m_playerDeath;
    private void Start()
    {
        m_oldMatPlayer = skinnedMeshRenderer.GetComponent<Material>();
        m_oldMatgun = gunrender.GetComponent<Material>();
        m_oldMateye = eyerender.GetComponent<Material>();
        m_playerDeath.m_OnReviveS += ResetMat;
    }
    public void Dissolve()
    {
        skinnedMeshRenderer.material = playermat;
        skinnedMeshRenderer.enabled = true;
        gunrender.enabled = true;
        gunrender1.enabled = true;
        eyerender.enabled = true;
        eyerender1.enabled = true;
        gunrender.material = gunmat;
        gunrender1.material = gunmat;
        eyerender.material = eyemat;
        eyerender1.material = eyemat;
        StopCoroutine(DissolveCoroutine());
        StartCoroutine(DissolveCoroutine());
    }
    IEnumerator DissolveCoroutine()
    {
        time += Time.deltaTime;
        skinnedMeshRenderer.material.SetFloat("_Dissapear_amount", time * speed);
        gunrender.material.SetFloat("_Dissapear_amount", time * speed);
        gunrender1.material.SetFloat("_Dissapear_amount", time * speed);
        eyerender.material.SetFloat("_Dissapear_amount", time * speed);
        eyerender1.material.SetFloat("_Dissapear_amount", time * speed);
        yield return new WaitForSeconds(0.1f);
        if (skinnedMeshRenderer.material.GetFloat("_Dissapear_amount") < max)
        {
            StartCoroutine(DissolveCoroutine());
        }
        else
        {
            skinnedMeshRenderer.material.SetFloat("_Dissapear_amount", 0);
            gunrender.material.SetFloat("_Dissapear_amount", 0);
            gunrender1.material.SetFloat("_Dissapear_amount", 0);
            eyerender.material.SetFloat("_Dissapear_amount", 0);
            eyerender1.material.SetFloat("_Dissapear_amount", 0);
            skinnedMeshRenderer.enabled = false;
            gunrender.enabled = false;
            gunrender1.enabled = false;
            eyerender.enabled = false;
            eyerender1.enabled = false;
            time = 0;
        }
    }
    public void ResetMat()
    {
        skinnedMeshRenderer.material = m_oldMatPlayer;
        gunrender.material = m_oldMatgun;
        gunrender1.material = m_oldMatgun;
        eyerender.material = m_oldMateye;
        eyerender1.material = m_oldMateye;
    }
}
