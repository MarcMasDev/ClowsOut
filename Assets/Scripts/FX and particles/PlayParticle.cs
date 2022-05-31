using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private bool shake = false;
    [SerializeField] float duration = 1f;
    [SerializeField] float magnitude = 0.4f;
    //eo
    public void PlayParticles()
    {
        if (shake) {
            GameManager.GetManager().GetCameraShake().Shake(magnitude, duration,transform); 
        }
    
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
    }
}
