using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    private ParticleSystem m_system;
    IEnumerator Start()
    {
        m_system = GetComponent<ParticleSystem>();
        yield return new WaitUntil(() => !m_system.isEmitting);
        Destroy(gameObject);
    }
}
