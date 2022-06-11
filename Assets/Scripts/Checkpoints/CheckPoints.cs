using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    public static CheckPoints m_instance;

    public Transform m_lastCheckpoint;
    private void Start()
    {
       
        GameManager.GetManager().SetCheckpointsManager(this);
    }
    public void LastCheckpoint(Transform respawnPos)
    {
        m_lastCheckpoint = respawnPos;
    }
    
}
