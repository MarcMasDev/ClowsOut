using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickBulletMachine : MonoBehaviour,IPointerClickHandler
{
    public CinemachineVirtualCamera m_Camera;
    public BulletMachine m_BulletMachine;
    public static Action DestroyBullets;
    public void OnPointerClick(PointerEventData eventData)
    {
        //GameManager.GetManager().GetCameraManager().m_SwitchCam.SwitchToNotAimingCamera(); 
        //GameManager.GetManager().GetCanvasManager().ShowIngameMenu();
        //GameManager.GetManager().GetCameraManager().m_CurrentBulletMenu = null;
        //m_BulletMachine.m_IsMenu = false;
    }
    public void OnAcceptBulletSwitchCam()
    {
        GameManager.GetManager().GetCameraManager().m_SwitchCam.SwitchToNotAimingCamera();
        GameManager.GetManager().GetCanvasManager().ShowIngameMenu();
        GameManager.GetManager().GetCameraManager().m_CurrentBulletMenu.Priority = 0;
        GameManager.GetManager().GetCameraManager().m_CurrentBulletMenu = null;
        GameManager.GetManager().GetCameraManager().m_MediumCamera.Priority = 10;
        m_BulletMachine.m_IsMenu = false;
        // GameManager.GetManager().GetCameraManager().m_SwitchCam.SwitchToNotAimingCamera();
        //GameManager.GetManager().GetCanvasManager().ShowReticle();
        DestroyBullets?.Invoke();
    } 
    public void OnAcceptRestTimeScale()
    {
       // GameManager.GetManager().GetCanvasManager().ShowIngameMenu();
    }

    
}
