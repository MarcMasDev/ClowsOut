using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    [SerializeField]
    InputManager m_InputManager;
    [SerializeField]
    CinemachineVirtualCamera m_aimCamera;
    [SerializeField]
    int m_IncreseCamPriority = 10;
    private void OnEnable()
    {
        m_InputManager.OnStartAiming += SwitchToAimCamera;
        m_InputManager.OnStopAiming += SwitchToThirdCamera;
    }
    private void OnDisable()
    {
        m_InputManager.OnStartAiming -= SwitchToAimCamera;
        m_InputManager.OnStopAiming -= SwitchToThirdCamera;
    }

    public void SwitchToAimCamera()
    {
        m_aimCamera.Priority += m_IncreseCamPriority;
    }
    public void SwitchToThirdCamera()
    {
        m_aimCamera.Priority -= m_IncreseCamPriority;
    }
}
