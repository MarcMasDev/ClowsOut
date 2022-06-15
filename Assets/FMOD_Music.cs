using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FMOD_Music : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter[] fmodEvent;
    [SerializeField] private float speedFade;
    private float i = 0;
    private bool started = false;
    private int index = 0;
    void Start()
    {
    }

    public void StartMusic()
    {
        if (!started)
        {
            started = true;
            i = 0;
            if (index >= fmodEvent.Length)
            {
                index = 0;
            }
            fmodEvent[index].SetParameter("End", 0);
            fmodEvent[index].Play();
        }

    }
    public void EndMusic()
    {
        StartCoroutine(EndTrack());
    }
    private IEnumerator EndTrack()
    {
        i = 0.1f;
        while (i<1)
        {
            fmodEvent[index].SetParameter("End", i);
            yield return new WaitForSeconds(speedFade * Time.deltaTime);
            i += 0.01f;

        }
        fmodEvent[index].SetParameter("End", 2);
        started = false;
        index++;
    }
}
