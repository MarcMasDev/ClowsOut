using System.Collections;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    private ParticleSystem m_system;
    IEnumerator Start()
    {
        m_system = GetComponent<ParticleSystem>();
        yield return new WaitUntil(() => m_system.IsAlive());
        yield return new WaitWhile(() => m_system.IsAlive());
        Destroy(gameObject);
    }
}
