using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader _instance;
   

    public static SceneLoader Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            _instance = this;
        }
    }
    public string[] LevelNames;

    public string LoadingSceneName;

    public void LoadLevel(int level)
    {
        LoadingSceneName = LevelNames[level];

        SceneManager.LoadScene("Loading");
        LoadSceneAsync(LoadingSceneName);
    }

    private void LoadSceneAsync(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }
    public void LoadWithLoadingScene(int scene)//TODO
    {
        //First load loading scene and save in var
        //also load loading scene
        AsyncOperation LoadLevel = SceneManager.LoadSceneAsync(scene);
        //LoadLevel.progress =1 loafing finish => update progresion bar
        LoadLevel.completed += (asyncOperation) =>
        {
            //If we need to set something when finish
        };

    }
   
}
