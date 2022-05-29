using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private bool shake = false;
    [SerializeField] float duration = 0.15f;
    [SerializeField] float magnitude = 0.4f;
    public void PlayParticles()
    {
        if (shake) {
            GameManager.GetManager().GetCameraShake().Shake(magnitude, duration); 
        }
    
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
    }
}
