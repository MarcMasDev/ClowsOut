using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class RestartElements : MonoBehaviour
{
    //public static RestartElements m_Instance;
    public List<IRestart> m_RestartElements = new List<IRestart>();
    public bool m_Restart = false;
    
    private void Awake()
    {
        GameManager.GetManager().SetRestartElements(this);
    }
    private void Update()
    {
        if (m_Restart)
        {
            Restart();
        }
    }
    public void addRestartElement(IRestart restart)
    {
        m_RestartElements.Add(restart);
    }
    public void Restart()
    {
        print("restart "+ m_RestartElements.Count);
        for (int i = 0; i < m_RestartElements.Count; i++)
        {
            m_RestartElements[i].Restart();
        }
    }
}
