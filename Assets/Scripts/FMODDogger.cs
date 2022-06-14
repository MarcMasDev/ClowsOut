using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODDogger : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter m_Hit;
    [SerializeField] private StudioEventEmitter m_Footstep;
    [SerializeField] private StudioEventEmitter m_ShootNormal;
    public void FootStep()
    {
        m_Footstep?.Play();
    }
    public void Hit()
    {
        m_Hit?.Play();
    }
    public void Shoot()
    {
        m_ShootNormal?.Play();
    }
}
