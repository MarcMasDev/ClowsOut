using UnityEngine;
using FMODUnity;

public class PlayParticle : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter emitter;
    [SerializeField] private bool parentSetter = false;
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private bool shake = false;
    [SerializeField] float duration = 1f;
    [SerializeField] float magnitude = 0.4f;
    //eo
    private void Start()
    {

    }
    public void PlayParticles()
    {
        if (parentSetter)
        {
            transform.SetParent(null);
        }
        if (emitter)
        {
            emitter.Play();
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
