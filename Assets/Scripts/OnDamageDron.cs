using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class OnDamageDron : MonoBehaviour
{
    VisualEffect m_visualEffect;
    [SerializeField]
    HealthSystem m_hp;
    private void OnEnable()
    {
        m_hp.m_OnHit += OnHit;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_visualEffect = GetComponent<VisualEffect>();
        m_visualEffect.Stop();
    }

    void OnHit(float f)
    {
        m_visualEffect.Play();
        StartCoroutine(DesactiveWithDelay());

    }
    IEnumerator DesactiveWithDelay()
    {
        yield return new WaitForSeconds(0.02f);
        m_visualEffect.Stop();
    }
}
