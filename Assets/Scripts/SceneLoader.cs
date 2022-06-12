using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private SceneLoader _instance;

    [Tooltip("dont touch this array. Look LevelData name levels")]
    [SerializeField]private string[] LevelNames;
    [SerializeField] private string LoadingSceneName;
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
        LevelNames = GameManager.GetManager().GetLevelData().m_SceneNames;
    }
   

    /// <summary>
    /// Load desired scene with out loading scene.
    /// </summary>
    /// <param name="level"></param>
    /// 
    public void LoadLevel(int level)
    {
        if (LoadingSceneName != LevelNames[level] && LevelNames.Length>level)
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
        if (LoadingSceneName != LevelNames[level] && LevelNames.Length > level)
        {
            LoadingSceneName = LevelNames[level];
            Debug.Log(LevelNames[level] + "scene loaded with exit");
            StartCoroutine(LoadLoadingScene(level));
        }
        else
            Debug.Log(level + " level doesn't exit or is already loaded.");
    }
    IEnumerator LoadLoadingScene(int scene)//TODO
    {
        //First load loading scene and save in var
        //also load loading scene
        SceneManager.LoadSceneAsync("Loading");
        yield return new WaitForSeconds(0.5f);
        TextEffects l_effects = FindObjectOfType<TextEffects>();
        AsyncOperation LoadLevel = SceneManager.LoadSceneAsync(scene);

        LoadLevel.allowSceneActivation = false;
        Debug.Log("Pro :" + LoadLevel.progress);
        //LoadLevel.progress =1 loafing finish => update progresion bar 
        while (!LoadLevel.isDone)
        { 
            Debug.Log("Pro :" + LoadLevel.progress);
            l_effects.m_TextPercentatge.text = "Loading progress: " + (LoadLevel.progress * 100) + "%";
            // Check if the load has finished
            if (LoadLevel.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
               // l_effects.m_TextPercentatge.text = "Press the space bar to continue";
                //Wait to you press the space key to activate the Scene
                //////////////if (Input.GetKeyDown(KeyCode.Space))
                //////////////    //Activate the Scene
                LoadLevel.allowSceneActivation = true;
            }
            yield return null;
        }
        LoadLevel.completed += (asyncOperation) =>
        {
            //If we need to set something when finish
        };
    }
}
