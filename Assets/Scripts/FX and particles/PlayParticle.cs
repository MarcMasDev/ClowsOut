using UnityEngine;
using FMODUnity;

public class PlayParticle : MonoBehaviour
{
    private StudioEventEmitter emitter;
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private bool shake = false;
    [SerializeField] float duration = 1f;
    [SerializeField] float magnitude = 0.4f;
    //eo
    private void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();
    }
    public void PlayParticles()
    {
        if (emitter)
        {
            emitter.Play();
            emitter = null;
        }

        if (shake) {
            GameManager.GetManager().GetCameraShake().Shake(magnitude, duration,transform); 
        }
        
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
    }
}
