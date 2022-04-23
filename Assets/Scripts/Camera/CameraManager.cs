using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] public CinemachineBrain m_CinemachineBrain;

    private static CameraManager m_Instance = null;

    public static CameraManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType<CameraManager>();
            }
            return m_Instance;
        }
    }

    public void CameraFixedUpdate()
    {
        m_CinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
    }
    public void CameraLateUpdate()
    {
        m_CinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
    }
}
