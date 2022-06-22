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
    private void Update()
    {
        Debug.Log("FC: " + m_CamManager.m_FarCamera.Priority + ", " + "AC: " + m_CamManager.m_FarCamera.Priority + ", " +
            "MC: " + m_CamManager.m_FarCamera.Priority + ", " + "LC: " + m_CamManager.m_LoadingCamera.Priority);
        if (m_CamManager.m_CurrentBulletMenu)
        {
            m_CamManager.m_CurrentBulletMenu.Priority = 20;
            StartCoroutine(EndFrame());
            Debug.Log("MenuC: " + m_CamManager.m_CurrentBulletMenu.Priority);
        }
    }
    public void SwitchInitCam()
    {
        m_CamManager.m_LoadingCamera.Priority = 0;
        m_CamManager.m_FarCamera.Priority = 0;
        m_CamManager.m_AimCamera.Priority = 0;
        m_CamManager.m_MediumCamera.Priority = 10;
    }
    public void SwitchToBulletMenuCamera()
    {
        m_CamManager.m_FarCamera.Priority = 0;
        m_CamManager.m_AimCamera.Priority = 0;
        m_CamManager.m_MediumCamera.Priority = 0;
        m_CamManager.m_LoadingCamera.Priority = 0;
        m_CamManager.m_CurrentBulletMenu.Priority = m_CamManager.m_IncreseCamPriority;
    }
    public void SwitchToAimCamera()
    {
        if (!m_CamManager.m_Locked)
        {
            //GameManager.GetManager().GetCameraManager().m_CameraShake.Priority = 0;
            m_CamManager.m_AimCamera.Priority = m_CamManager.m_IncreseCamPriority;
            m_CamManager.m_MediumCamera.Priority = 0;
            m_CamManager.m_FarCamera.Priority = 0;
            m_CamManager.m_LoadingCamera.Priority = 0;
        }
    }
    public void SwitchToNotAimingCamera()
    {
        if (!m_CamManager.m_CurrentBulletMenu)
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
                m_CamManager.m_LoadingCamera.Priority = 0;
                m_CamManager.m_MediumCamera.Priority = m_CamManager.m_IncreseCamPriority;

                //GameManager.GetManager().GetCameraManager().m_CameraShake.Priority = 0;
            }
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
    IEnumerator EndFrame()
    {
        yield return new WaitForEndOfFrame();
        m_CamManager.m_CurrentBulletMenu.Priority = 20;
    }
}
