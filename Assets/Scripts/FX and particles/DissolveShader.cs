using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveShader : MonoBehaviour
{

    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float max = 1f;
    [SerializeField] private Material mat;
    private float time = 0;
    public void Dissolve()
    {
        StartCoroutine(ExampleCoroutine());
    }
    IEnumerator ExampleCoroutine()
    {
        time += Time.deltaTime;
        mat.SetFloat("_Dissapear_amount", time * speed);
        yield return new WaitForSeconds(0.1f);
        if (mat.GetFloat("_Dissapear_amount") < max)
        {
            StartCoroutine(ExampleCoroutine());
        }
        else
        {
            gameObject.SetActive(false);
            mat.SetFloat("_Dissapear_amount", 0);
            time = 0;
        }
    }
}
