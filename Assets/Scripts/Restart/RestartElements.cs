using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class RestartElements : MonoBehaviour
{
    //public static RestartElements m_Instance;
    public List<IRestart> m_RestartElements = new List<IRestart>();
    public List<GameObject> m_RestartElementsGB = new List<GameObject>();
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
    public void addRestartElement(IRestart restart, Transform transformRest)
    {
        
        m_RestartElements.Add(restart);
        m_RestartElementsGB.Add(transformRest.gameObject);
    }
    public void Restart()
    {
        print("restart "+ m_RestartElements.Count);
        for (int i = 0; i < m_RestartElements.Count; i++)
        {
            if (m_RestartElementsGB[i])
            {
                m_RestartElements[i]?.Restart();
            }
               
        }
    }
}
