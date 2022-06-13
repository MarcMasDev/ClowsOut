using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private SceneLoader _instance;

    [Tooltip("dont touch this array. Look LevelData name levels")]
    private string[] m_LevelNames;
    [SerializeField] private string m_LoadingSceneName;
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
        m_LevelNames = GameManager.GetManager().GetLevelData().m_SceneNames;
    }
    /// <summary>
    /// Load desired scene with out loading scene.
    /// </summary>
    /// <param name="level"></param>
    /// 
    public void LoadLevel(int level)
    {
        if (m_LoadingSceneName != m_LevelNames[level] && m_LevelNames.Length>level)
        {
            m_LoadingSceneName = m_LevelNames[level];
            Debug.Log(m_LevelNames[level] + "scene loaded with exit");
            GameManager.GetManager().GetLevelData().m_CurrentLevelPlayed = level;
            LoadSceneAsync(m_LoadingSceneName);
        }
        else
        Debug.Log(m_LevelNames[level] + "scene doesn't exit. Cant be loaded.");
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
        if (m_LoadingSceneName != m_LevelNames[level] && m_LevelNames.Length > level)
        {
            m_LoadingSceneName = m_LevelNames[level];
            Debug.Log(m_LevelNames[level] + " level scene loaded with exit");
            GameManager.GetManager().GetLevelData().m_CurrentLevelPlayed = level;
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
        AsyncOperation l_LoadLevel = SceneManager.LoadSceneAsync(scene);

        l_LoadLevel.allowSceneActivation = false;
        while (!l_LoadLevel.isDone)
        { 
            l_effects.m_TextPercentatge.text = "Loading progress: " + (l_LoadLevel.progress * 100) + " %";
            // Check if the load has finished
            if (l_LoadLevel.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                //l_effects.m_TextPercentatge.text = "Press the space bar to continue";
                //Wait to you press the space key to activate the Scene
                //////////////if (Input.GetKeyDown(KeyCode.Space))
                //////////////    //Activate the Scene
                l_LoadLevel.allowSceneActivation = true;
            }
            yield return null;
        }
        //LoadLevel.completed += (asyncOperation) =>
        //{
        //    //If we need to set something when finish
        //};
    }
}
