using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.Instance.OnStartAiming += SwitchToAimCamera;
        InputManager.Instance.OnStopAiming += SwitchToThirdCamera;
    }
    private void OnDisable()
    {
        InputManager.Instance.OnStartAiming -= SwitchToAimCamera;
        InputManager.Instance.OnStopAiming -= SwitchToThirdCamera;
    }

    public void SwitchToAimCamera()
    {
        if (!CameraManager.Instance.m_Locked)
        {
            CameraManager.Instance.m_aimCamera.Priority += CameraManager.Instance.m_IncreseCamPriority;
        }
    }
    public void SwitchToThirdCamera()
    {
        if (!CameraManager.Instance.m_Locked)
        {
            CameraManager.Instance.m_aimCamera.Priority -= CameraManager.Instance.m_IncreseCamPriority;
        }
    }
}
