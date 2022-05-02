using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartElements : MonoBehaviour
{
    public static RestartElements m_Instance;
    public List<IRestart> m_RestartElements;

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            m_RestartElements = new List<IRestart>();
        }
        else
        {
            GameObject.Destroy(this);
        }
    }
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
