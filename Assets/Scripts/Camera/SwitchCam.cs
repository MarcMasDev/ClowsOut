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
        if (!GameManager.GetManager().GetCameraManager().m_Locked)
        {
            GameManager.GetManager().GetCameraManager().m_aimCamera.Priority += GameManager.GetManager().GetCameraManager().m_IncreseCamPriority;
        }
    }
    public void SwitchToThirdCamera()
    {
        if (!GameManager.GetManager().GetCameraManager().m_Locked)
        {
            GameManager.GetManager().GetCameraManager().m_aimCamera.Priority -= GameManager.GetManager().GetCameraManager().m_IncreseCamPriority;
        }
    }
}
