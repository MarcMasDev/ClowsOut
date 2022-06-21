using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigController : MonoBehaviour
{
    public RigBuilder m_RigBuilder;
    public bool m_Reloading;
    public bool m_Wall;
    public bool m_Rotate;

    private void Update()
    {
        for (int i = 0; i < m_RigBuilder.layers.Count; i++)
        {
            m_RigBuilder.layers[i].active = true;
        }
        if (m_Wall)
        {
            m_RigBuilder.layers[1].active = false;
        }
        if (m_Reloading)
        {
            m_RigBuilder.layers[1].active = false;
        }
    }

    //public void Reload()
    //{
    //    m_Reloading = true;
    //}
    //public void StopReload()
    //{
    //    m_Reloading = false;
    //}
    public void EndRotate()
    {
        m_Rotate = false;
    }
}
