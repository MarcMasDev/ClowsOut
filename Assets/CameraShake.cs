using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public UnityEvent m_shock;
    private void Start()
    {
        Debug.Log("cam "+   gameObject.name);
    }
    public void Shake(float defaultIntensity, float time)
    {
        InvokeRepeating("", time, defaultIntensity);
        //Se llama por el game manager des de las explosiones (la script donde se llama se llama: PlayParticle)
    }
    void ShockWaveEvent()
    {
        m_shock.Invoke();
    }
}
