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

    public void LoadLevel(int level)
    {
        LoadingSceneName = LevelNames[level];

        //SceneManager.LoadScene("Loading");
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
        SceneManager.LoadScene("Loading");
        AsyncOperation LoadLevel = SceneManager.LoadSceneAsync(scene);
        //LoadLevel.progress =1 loafing finish => update progresion bar
        LoadLevel.completed += (asyncOperation) =>
        {
            //If we need to set something when finish
        };

    }
   
}
