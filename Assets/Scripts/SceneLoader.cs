using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private SceneLoader _instance;
   
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            GameManager.GetManager().SetSceneLoader(_instance=this);
        }
    }

    private void Start()
    {
        GameManager.GetManager().SetSceneLoader(this);
    }
    public string[] LevelNames;

    public string LoadingSceneName;

    /// <summary>
    /// Load desired scene with out loading scene.
    /// </summary>
    /// <param name="level"></param>
    public void LoadLevel(int level)
    {
        if (LoadingSceneName == LevelNames[level])
        {
            LoadingSceneName = LevelNames[level];
            Debug.Log(LevelNames[level] + "scene loaded with exit");
            LoadSceneAsync(LoadingSceneName);
        }
        else
        Debug.Log(LevelNames[level] + "scene doesn't exit. Cant be loaded.");
    }

    private void LoadSceneAsync(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }

    /// <summary>
    /// Load Scene with a loading scene to charge.
    /// When It finish the desired scene has been charged.
    /// </summary>
    /// <param name="scene"></param>
    public void LoadWithLoadingScene(int level)
    {
        if (LoadingSceneName == LevelNames[level])
        {
            LoadingSceneName = LevelNames[level];
            Debug.Log(LevelNames[level] + "scene loaded with exit");
            StartCoroutine(LoadLoadingScene(level));
        }
        else
            Debug.Log(level + "level doesn't exit. Cant be loaded.");
    }
    IEnumerator LoadLoadingScene(int scene)//TODO
    {
        //First load loading scene and save in var
        //also load loading scene
        SceneManager.LoadSceneAsync("Loading");
        yield return new WaitForSeconds(0.5f);
       // SceneManager.UnloadScene(SceneManager.GetActiveScene().buildIndex-1);
        AsyncOperation LoadLevel = SceneManager.LoadSceneAsync(scene);
        //LoadLevel.progress =1 loafing finish => update progresion bar 
        LoadLevel.completed += (asyncOperation) =>
        {
            //If we need to set something when finish
        };
    }
}
