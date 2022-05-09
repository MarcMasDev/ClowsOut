using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    private void OnEnable()
    {

        GameManager.GetManager().GetInputManager().OnStartAiming += SwitchToAimCamera;
        GameManager.GetManager().GetInputManager().OnStopAiming += SwitchToThirdCamera;
    }
    private void OnDisable()
    {
        GameManager.GetManager().GetInputManager().OnStopAiming -= SwitchToThirdCamera;
        GameManager.GetManager().GetInputManager().OnStartAiming -= SwitchToAimCamera;
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
