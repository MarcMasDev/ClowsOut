using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public TextEffects m_effects;
    [Tooltip("dont touch this array. Look LevelData name levels")]
    private string[] m_LevelNames;
    private string m_LoadingSceneName;
    private void Awake()
    {
        if (GameManager.GetManager().GetSceneLoader() == null)
        {
            GameManager.GetManager().SetSceneLoader(this);
            DontDestroyOnLoad(gameObject);
        }
        else if (GameManager.GetManager().GetSceneLoader() != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
          Time.timeScale = 1;
        
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
        if (m_LoadingSceneName != m_LevelNames[level] && m_LevelNames.Length > level)
        {
            GameManager.GetManager().GetLevelData().ResetTotalTime();
            m_LoadingSceneName = m_LevelNames[level];
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
            GameManager.GetManager().GetLevelData().m_CurrentLevelPlayed = level;
            StartCoroutine(LoadLoadingScene(level));
        }
        else
            Debug.Log(level + " level doesn't exit or is already loaded.");
    }
    IEnumerator LoadLoadingScene(int scene)
    {
        //First load loading scene and save in var
        //also load loading scene
        SceneManager.LoadSceneAsync("Loading");
        yield return new WaitForSecondsRealtime(0.5f);
        m_effects = FindObjectOfType<TextEffects>();
        AsyncOperation l_LoadLevel = SceneManager.LoadSceneAsync(scene);
        l_LoadLevel.allowSceneActivation = false;
        yield return new WaitForSecondsRealtime(1f);
        while (!l_LoadLevel.isDone)
        {
            m_effects.m_TextPercentatge.text = "Loading progress: " + Mathf.Round((l_LoadLevel.progress * 100)) + " %";

            // Check if the load has finished
            if (l_LoadLevel.progress >= 0.9f)
            {
                yield return new WaitForSecondsRealtime(3.5f);
                m_effects.StartNewScene();
                yield return new WaitForSecondsRealtime(2);
                l_LoadLevel.allowSceneActivation = true;
            }
            yield return null;
        }
        l_LoadLevel.completed += (asyncOperation) =>
        {
            GameManager.GetManager().GetLevelData().m_GameStarted = true;
        };
    }
}
