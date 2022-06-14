using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    private CameraManager m_CamManager;
    private void OnEnable()
    {
        GameManager.GetManager().GetInputManager().OnStartAiming += SwitchToAimCamera;
        GameManager.GetManager().GetInputManager().OnStopAiming += SwitchToNotAimingCamera;
    }
    private void OnDisable()
    {
        GameManager.GetManager().GetInputManager().OnStartAiming -= SwitchToAimCamera;
        GameManager.GetManager().GetInputManager().OnStopAiming -= SwitchToNotAimingCamera;
    }
    private void Start()
    {
        m_CamManager = GetComponent<CameraManager>();
    }
    public void SwitchToBulletMenuCamera()
    {
        m_CamManager.m_AimCamera.Priority = 0;
        m_CamManager.m_MediumCamera.Priority = 0;
        m_CamManager.m_FarCamera.Priority = 0;
        m_CamManager.m_MediumCamera.Priority = m_CamManager.m_IncreseCamPriority;
        
        //if (!m_CamManager.m_Locked )
        //{
        //    //GameManager.GetManager().GetCameraManager().m_CameraShake.Priority = 0;
        //    m_CamManager.m_AimCamera.Priority = 0;
            
        //    if (m_CamManager.m_CurrentBulletMenu != null)
        //    {
               
                
        //    }
        //}
    }
    public void SwitchToAimCamera()
    {
        if (!m_CamManager.m_Locked)
        {
            //GameManager.GetManager().GetCameraManager().m_CameraShake.Priority = 0;
            m_CamManager.m_AimCamera.Priority += m_CamManager.m_IncreseCamPriority;
            m_CamManager.m_MediumCamera.Priority = 0;
            m_CamManager.m_FarCamera.Priority = 0;
        }
    }
    public void SwitchToNotAimingCamera()
    {
        if (!m_CamManager.m_Locked)
        {
            if (m_CamManager.m_CurrentBulletMenu != null)
            {
                m_CamManager.m_CurrentBulletMenu.Priority = 0; 
            }
            m_CamManager.m_AimCamera.Priority = 0;
            m_CamManager.m_MediumCamera.Priority = 0;
            m_CamManager.m_FarCamera.Priority = 0;
            m_CamManager.m_MediumCamera.Priority += m_CamManager.m_IncreseCamPriority;

            //GameManager.GetManager().GetCameraManager().m_CameraShake.Priority = 0;
        }
    }
    public void SwitchToShakeCamera()
    {
        if (!m_CamManager.m_Locked)
        {
            m_CamManager.m_AimCamera.Priority = 0;
            
            //GameManager.GetManager().GetCameraManager().m_CameraShake.Priority += GameManager.GetManager().GetCameraManager().m_IncreseCamPriority;
        }
    }
}
