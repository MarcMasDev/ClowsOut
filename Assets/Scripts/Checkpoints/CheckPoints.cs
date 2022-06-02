using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    public static CheckPoints m_instance;

    public Transform m_lastCheckpoint;
    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            m_instance = this;

        }
    }
    public void LastCheckpoint(Transform respawnPos)
    {
        m_lastCheckpoint = respawnPos;
    }
    
}
