using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class RestartElements : MonoBehaviour
{
    //public static RestartElements m_Instance;
    public List<IRestart> m_RestartElements = new List<IRestart>();

    private void Awake()
    {
        GameManager.GetManager().SetRestartElements(this);
    }

    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += Init;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= Init;
    //}
    //public void Init(Scene scene, LoadSceneMode a)
    //{
    //    GameManager.GetManager().SetRestartElements(this);
    //}
    public void addRestartElement(IRestart restart)
    {
        
        m_RestartElements.Add(restart);

    }
    public void Restart()
    {
        for (int i = 0; i < m_RestartElements.Count; i++)
        {
            m_RestartElements[i].Restart(); 
        }
    }
}
