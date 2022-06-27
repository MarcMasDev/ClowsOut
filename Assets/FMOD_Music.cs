using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FMOD_Music : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter[] fmodEvent;
    [SerializeField] private float speedFade;
    private float i = 0, f = 1;
    private int index = 2;
    private static FMOD_Music _instance;
    void OnEnable()
    {
        Player_Death.m_OnDeathS += RestartMusic;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        Player_Death.m_OnDeathS -= RestartMusic;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Awake()
    {
        if (!_instance)
            _instance = this;

        else
            Destroy(gameObject);


        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        StartCoroutine(StartTrack(0));
    }
    public void StartMusic()
    {
        i = 0;
        if (index >= fmodEvent.Length)
        {
            index = 2;
        }
        fmodEvent[index].SetParameter("MusicVolumeSet", 0);
        fmodEvent[index].Play();
        StartCoroutine(EndTrack(1));
    }
    public void EndMusic()
    {
        StartCoroutine(EndTrack(index));
        StartCoroutine(StartTrack(1));
    }
    private IEnumerator EndTrack( int indx)
    {
        i = 0.1f;
        while (i<1)
        {
            fmodEvent[indx].SetParameter("MusicVolumeSet", i);
            yield return new WaitForSeconds(speedFade * Time.deltaTime);
            i += 0.01f;

        }
        fmodEvent[indx].SetParameter("MusicVolumeSet", 2);
        if (indx != 0 && indx!= 1)
            index++;
    }
    private IEnumerator StartTrack(int indx)
    {
        if (!fmodEvent[indx].IsPlaying())
        {
            fmodEvent[indx].Play();
        }
        f = 1f;
        while (f > 0)
        {
            fmodEvent[indx].SetParameter("MusicVolumeSet", f);
            yield return new WaitForSeconds(speedFade * Time.deltaTime);
            f -= 0.01f;
            print(f);
        }
        fmodEvent[indx].SetParameter("MusicVolumeSet", 0);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if ((scene.buildIndex == 0 || scene.buildIndex == 1) && index != 0)
        {
            StartCoroutine(EndTrack(index));

            StartCoroutine(StartTrack(0));
        }
        else if (scene.buildIndex == 2)
        {
            StartCoroutine(EndTrack(0));
            StartCoroutine(StartTrack(1));
        }
    }
    private void RestartMusic()
    {
        StartCoroutine(StartTrack(1));
        StartCoroutine(EndTrack(index));
    }

}
