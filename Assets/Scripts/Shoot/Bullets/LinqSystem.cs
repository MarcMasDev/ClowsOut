using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinqSystem : MonoBehaviour
{
    LinqSystem m_Instance;
    
    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            GameObject.Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
